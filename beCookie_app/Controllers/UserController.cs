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
        public IEnumerable<User> Get(int id)
        {   
            var context = new wypxrkenContext();
            if (id <= 0)
            {    
                foreach (var elem in context.Users)
                    yield return elem;
            }
            else
            {
                foreach (var elem in context.Users.Where(user => user.Id == id))
                    yield return elem;
            }
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
            return Ok("ѕользователь добавлен");
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
    }
}