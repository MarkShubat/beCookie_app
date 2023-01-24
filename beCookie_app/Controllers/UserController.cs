using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("GetUserById")]
        public User GetUserById(int id)
        {
            var context = new wypxrkenContext();
            return context.Users.Where(faq => faq.Id == id).FirstOrDefault();
        }

        [HttpGet]
        [Route("GetUsers")]
        public IEnumerable<User> GetFaqs()
        {
            var context = new wypxrkenContext();
            return context.Users;
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult Add(string email, string phoneNumber, string name )
        {
            var context = new wypxrkenContext();
            var user = new User
            {
                Email = email,
                PhoneNumber = phoneNumber,
                Name = name
            };
            context.Users.Add(user);
            context.SaveChangesAsync();
            return Ok("Пользователь добавлен");
        }

        [HttpDelete]
        [Route("DeleteAll")]
        public IActionResult Delete()
        {
            var context = new wypxrkenContext();
            foreach (var elem in context.Users)
            {
                context.Users.Remove(elem);
            }
            context.SaveChangesAsync();
            return Ok("записи удалены");
        }

        [HttpDelete]
        [Route("DeleteById")]
        public IActionResult DeleteById(int id)
        {
            var context = new wypxrkenContext();
            foreach (var elem in context.Users)
            {
                if(elem.Id == id)
                    context.Users.Remove(elem);
            }
            context.SaveChangesAsync();
            return Ok("записи удалены");
        }
    }
}