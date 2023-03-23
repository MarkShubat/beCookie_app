using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using beCookie_app.Methods;
using System.Text;
using System.Net.NetworkInformation;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "FetGlXbyWgrdmSU68bYEqpzR0SCAccJwpwyiUKGV",
            BasePath = "https://becookieappchat-default-rtdb.europe-west1.firebasedatabase.app"
        };
        IFirebaseClient client;

        [HttpGet]
        [Route("GetEvents")]
        public IEnumerable<EventInfo> GetEvents(int currentUserId)
        {
            var context = new wypxrkenContext();
            var context1 = new wypxrkenContext();
            var context2 = new wypxrkenContext();
            foreach(var elem in context.Events)
            {
                var members = context1.Members.Where(m => m.EventId == elem.Id);
                var IsUserMember = members.Where(m => m.UserId == currentUserId).Any();
                var status = IsUserMember ? currentUserId == elem.AdminId ? "Owner" : "Member" : "NotMember";
                var admin = context2.Users.Where(user => user.Id == elem.AdminId).FirstOrDefault();
                yield return new EventInfo(elem, members.Count(), status, admin);
            }
        }

        [HttpGet]
        [Route("GetEventsWhereUserIsMember")]
        public IEnumerable<EventInfo> GetEventsWhereUserIsMember(int currentUserId)
        {
            var context = new wypxrkenContext();
            var context1 = new wypxrkenContext();
            var context2 = new wypxrkenContext();
            foreach (var elem in context.Events)
            {
                var members = context1.Members.Where(m => m.EventId == elem.Id);
                var IsUserMember = members.Where(m => m.UserId == currentUserId).Any();
                var status = IsUserMember ? currentUserId == elem.AdminId ? "Owner" : "Member" : "NotMember";
                var admin = context2.Users.Where(user => user.Id == elem.AdminId).FirstOrDefault();
                if(status != "NotMember")
                    yield return new EventInfo(elem, members.Count(), status, admin);
            }
        }

        [HttpGet]
        [Route("GetEventById")]
        public EventInfo GetEventById(int id, int currentUserId)
        {
            var context = new wypxrkenContext();
            var context1 = new wypxrkenContext();
            var context2 = new wypxrkenContext();
            var elem = context.Events.Where(e => e.Id == id).FirstOrDefault();
            var members = context1.Members.Where(m => m.EventId == elem.Id);
            var IsUserMember = members.Where(m => m.UserId == currentUserId).Any();
            var status = IsUserMember ? currentUserId == elem.AdminId ? "Owner" : "Member" : "NotMember";
            var admin = context2.Users.Where(user => user.Id == elem.AdminId).FirstOrDefault();
            return new EventInfo(elem, members.Count(), status,admin);
        }

        [HttpPost]
        [Route("AddEvent")]
        public IActionResult Add(Event e)
        {
            var context = new wypxrkenContext();
            var event1 = new Event
            {
                Title = e.Title,
                Description = e.Description,
                Schedule = e.Schedule,
                AdminId = e.AdminId,
                Adress = e.Adress,
                Date = DateTimeConverter.GetDateTimeString(),
                Location = e.Location,
                Type = e.Type
            };
            context.Events.Add(event1);
            context.SaveChanges();
            var context1 = new wypxrkenContext();
            var eventId = context1.Events.Where(e => e.AdminId == event1.AdminId && event1.Title == e.Title).FirstOrDefault().Id;
            context1.Members.Add(new Member { EventId = eventId, UserId = e.AdminId });
            context1.SaveChanges();
            return Ok("Событие добавлено");
        }

        [HttpPost]
        [Route("WriteMessage")]
        public IActionResult WriteMessage(string userId, string Text, string eventId)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var context = new wypxrkenContext();
                var data = new Message
                {
                    userId = userId,
                    text = Text,
                    eventId = eventId,
                    date = DateTimeConverter.GetDateTimeString(),
                    userName = context.Users.Where(u => u.Id == Convert.ToInt32(userId)).FirstOrDefault().Name,
                    edited = false
                };
                PushResponse response = client.Push("Messages/", data);
                data.id = response.Result.name;
                SetResponse setResponse = client.Set("Messages/" + data.id, data);

                if (setResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ModelState.AddModelError(string.Empty, "Added Succesfully");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong!!");
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return Ok("сообщение отправлено");
        }

        [HttpPost]
        [Route("EditMessage")]
        public IActionResult EditMessage(string id, string text)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Messages/" + id);
            var data = JsonConvert.DeserializeObject<Message>(response.Body);
            data.text = text;
            data.edited = true;
            SetResponse response1 = client.Set("Messages/" + id, data);
            return Ok("сообщение изменено");

        }

        [HttpDelete]
        [Route("DeleteAll")]
        public IActionResult Delete()
        {
            var context = new wypxrkenContext();
            foreach (var elem in context.Events)
            {
                context.Events.Remove(elem);
            }
            context.SaveChangesAsync();
            return Ok("записи удалены");
        }

        [HttpGet]
        [Route("GetPointInfo")]
        public string GetPointInfo(string x, string y)
        {
            var sb = new StringBuilder("https://geocode-maps.yandex.ru/1.x/?apikey=a9ee6d10-90f7-4df7-b755-8d2b210a9aa1&format=json&geocode=");
            sb.Append(x);
            sb.Append(",");
            sb.Append(y);
            return GetPointName(sb.ToString()).Result;

        }

        public static async Task<string> GetPointName(string request)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Rootobject>(responseBody).response.GeoObjectCollection.featureMember[0].GeoObject.metaDataProperty.GeocoderMetaData.text;
            return result;
        }

        [HttpGet]
        [Route("GetAdressLoc")]
        public Loc GetAdressLoc(string adress)
        {
            (string, string) street_house = new (adress.Split()[0], adress.Split()[1]);
            var sb = new StringBuilder("https://geocode-maps.yandex.ru/1.x/?apikey=a9ee6d10-90f7-4df7-b755-8d2b210a9aa1&format=json&geocode=Екатеринбург+");
            sb.Append(street_house.Item1);
            sb.Append("+");
            sb.Append(street_house.Item2);
            return GetPointLocation(sb.ToString()).Result;
        }

        public static async Task<Loc> GetPointLocation(string request)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            var loc = new Loc();     
            var result = JsonConvert.DeserializeObject<Rootobject>(responseBody).response.GeoObjectCollection.featureMember[0].GeoObject.Point.pos;
            loc.x = result.Split()[0];
            loc.y = result.Split()[1];
            return loc;
        }
    }

    public class EventInfo: Event
    {
       public int MembersCount { get; set; }
       public string UserStatus { get; set; }


        public EventInfo (Event e, int membersCount, string status, User admin)
        {
            Id = e.Id;
            AdminId = e.AdminId;
            MembersCount = membersCount;
            UserStatus = status;
            Title = e.Title;
            Type = e.Type;
            Description = e.Description;
            Schedule = e.Schedule;
            Date = e.Date;
            Adress = e.Adress;
            Location = e.Location;
            Admin = admin;
        }
    }
    public class Loc
    {
        public string x { get; set; }
        public string y { get; set; }
    }
}