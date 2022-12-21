using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {       
        [HttpGet]
        [Route("GetEventById")]
        public IEnumerable<Event> Get(int id)
        {   
            var context = new wypxrkenContext();
            if (id <= 0)
            {    
                foreach (var elem in context.Events)
                    yield return elem;
            }
            else
            {
                foreach (var elem in context.Events.Where(Event => Event.Id == id))
                    yield return elem ;
            }
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
}