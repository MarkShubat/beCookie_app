using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PointController : ControllerBase
    {       

        [HttpGet]
        [Route("GetPointById")]
        public Point GetPointById(int id)
        {
            var context = new wypxrkenContext();
            return context.Points.Where(faq => faq.Id == id).FirstOrDefault();
        }

        [HttpGet]
        [Route("GetPoints")]
        public IEnumerable<Point> GetPoints()
        {
            var context = new wypxrkenContext();
            return context.Points;
        }

        [HttpPost] //работай!!!
        [Route("GetPointByType")]
        public IEnumerable<Point> GetByType(PointType pointType)
        {
            var type = pointType.type;
            var context = new wypxrkenContext();
            type = type.Replace(" ", "");
            var typePart = type.Split(",");
            if (context.Points.Count() != 0)
            {           
                foreach(var elem in context.Points)
                {
                    var types = elem.Types.Replace(" ", "").Split(",");
                    foreach(var elem1 in typePart)
                    {
                        if (types.Contains(elem1)) yield return elem;
                    }
                }
            }
        }

        [HttpPost]
        [Route("AddPoint")]
        public IActionResult Add(string title, string desc, string schedule, string type, string adress, string phone, string date, double latitude, double longitude)
        {
            var context = new wypxrkenContext();
            var point = new Point
            {
                Title = title,
                Description = desc,
                Schedule = schedule,
                Types = type,
                Adress = adress,
                PhoneNumber = phone,
                Location = new NpgsqlTypes.NpgsqlPoint(latitude, longitude),
                Date = date
            };
            context.Points.Add(point);           
            context.SaveChangesAsync();
            return Ok("Точка добавлен");
        }

        [HttpDelete]
        [Route("DeleteAll")]
        public IActionResult Delete()
        {
            var context = new wypxrkenContext();
            foreach (var elem in context.Points)
            {
                context.Points.Remove(elem);
            }
            context.SaveChangesAsync();
            return Ok("записи удалены");
        }

        [HttpDelete]
        [Route("DeleteById")]
        public IActionResult DeleteById(int id)
        {
            var context = new wypxrkenContext();
            var point = context.Points.Where(point => point.Id == id).FirstOrDefault();
            context.Points.Remove(point);
            context.SaveChangesAsync();
            return Ok("записи удалены");
        }
    }
    public class PointType
    {
        public string type { get; set; }
    }
}