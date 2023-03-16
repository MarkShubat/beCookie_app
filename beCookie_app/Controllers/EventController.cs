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
        public IEnumerable<Event> GetEvents()
        {   
            var context = new wypxrkenContext(); 
            return context.Events;
        }

        [HttpGet]
        [Route("GetEventById")]
        public Event GetEventById(int id)
        {
            var context = new wypxrkenContext();
            return context.Events.Where(item => item.Id == id).FirstOrDefault();
        }

        [HttpPost]
        [Route("AddEvent")]
        public IActionResult Add(string title, string desc, string schedule, string adress, string date, int adminId)
        {
            var context = new wypxrkenContext();
            var event1 = new Event
            {
                Title = title,
                Description = desc,
                Schedule = schedule,
                AdminId = adminId,
                Adress = adress,
                Date = date
            };
            context.Events.Add(event1);
            context.SaveChangesAsync();
            return Ok("Событие добавлено");
        }

        [HttpPost]
        [Route("WriteMessage")]
        public IActionResult WriteMessage(string userId, string Text, string eventId)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                var data = new Message
                {
                    UserId = userId,
                    Text = Text,
                    EventId = eventId,
                    Date = DateTimeConverter.GetDateTimeString()
                };
                PushResponse response = client.Push("Messages/", data);
                data.Id = response.Result.name;
                SetResponse setResponse = client.Set("Messages/" + data.Id, data);

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

        [HttpGet]
        [Route("GetMessages")]
        public IEnumerable<MessageInfo> Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Messages");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Message>();
            if (data != null)
            {
                foreach (var item in data)
                {
                    var message = JsonConvert.DeserializeObject<Message>(((JProperty)item).Value.ToString());
                    var context = new wypxrkenContext();
                    var user = context.Users.Where(user => user.Id == Convert.ToInt32(message.UserId)).FirstOrDefault();
                    yield return new MessageInfo(user, message.Text, message.Date);
                }
            }
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

    public class MessageInfo
    {       
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserAvatar { get; set; }
        public string? Text { get; set; }
        public string? Date { get; set; }

        public MessageInfo(User user, string? text, string? date)
        {
            UserId = user.Id;
            UserName = user.Name;
            Text = text;
            Date = date;
        }
    }
}