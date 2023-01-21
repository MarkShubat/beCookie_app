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

        [HttpGet]
        [Route("GetPointByType")]
        public IEnumerable<Point> GetByType(int type)
        {
            var context = new wypxrkenContext();
            if(context.Points.Count() != 0)
                return context.Points.Where(point => point.Types.Split().Contains(type.ToString()));
            return context.Points;
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
    }
}