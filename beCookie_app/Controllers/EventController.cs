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
        public IActionResult Add(string title, string desc, string schedule, string adress, int adminId)
        {
            var context = new wypxrkenContext();
            var event1 = new Event
            {
                Title = title,
                Description = desc,
                Schedule = schedule,
                AdminId = adminId,
                Adress = adress,
                Date = DateTimeConverter.GetDateTimeString()
            };
            context.Events.Add(event1);
            context.SaveChanges();
            var context1 = new wypxrkenContext();
            var eventId = context1.Events.Where(e => e.AdminId == event1.AdminId && event1.Title == e.Title).FirstOrDefault().Id;
            context1.Members.Add(new Member { EventId = eventId, UserId = adminId });
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
}