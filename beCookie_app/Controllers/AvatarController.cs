using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvatarController : ControllerBase
    {       
        [HttpGet]
        [Route("GetAvatarById")]
        public IEnumerable<Avatar> GetMemberById(int id)
        {   
            var context = new wypxrkenContext();
            return context.Avatars;
        }

        [HttpPost]
        [Route("AddAvatar")]
        public IActionResult GetEventMembers(int id)
        {
            var context = new wypxrkenContext();
            var user = context.Users.Where(x => x.Id == Data.currentUser.Id).FirstOrDefault();
            user.AvatarId = id;
            context.SaveChanges();
            return Ok("Аватар добавлен");
        }

        [HttpDelete]
        [Route("DeleteAll")]
        public IActionResult Delete()
        {
            var context = new wypxrkenContext();
            foreach(var elem in context.Avatars)
            {
                context.Avatars.Remove(elem);
            }
            context.SaveChangesAsync();
            return Ok("записи удалены");
        }
    }
}