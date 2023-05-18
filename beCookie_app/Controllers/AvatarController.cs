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
        [Route("GetAvatars")]
        public IEnumerable<Avatar> GetAvtars()
        {   
            var context = new wypxrkenContext();
            return context.Avatars;
        }

        [HttpGet]
        [Route("GetAvatarById")]
        public Avatar GetAvtarById(int id)
        {
            var context = new wypxrkenContext();
            return context.Avatars.Where(item => item.Id == id).FirstOrDefault();
        }

        [HttpDelete]
        [Route("DeleteById")]
        public IActionResult DeleteById(int id)
        {
            var context = new wypxrkenContext();
            var elem = context.Avatars.Where(e => e.Id == id).FirstOrDefault();
            context.Avatars.Remove(elem);
            context.SaveChangesAsync();
            return Ok("записи удалены");
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