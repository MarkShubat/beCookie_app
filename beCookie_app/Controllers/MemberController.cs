using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemberController : ControllerBase
    {
       
        [HttpGet]
        [Route("GetEventMembers")]
        public IEnumerable<MemberInfo> GetEventMembers(int id)
        {
            var context = new wypxrkenContext();
            var context1 = new wypxrkenContext();

            foreach(var elem in context.Members.Where(member => member.EventId == id))
            {
                var user = context1.Users.Where(u => u.Id == elem.UserId).FirstOrDefault();
                yield return new MemberInfo(elem, user);
            }
        }
    }
    public class MemberInfo : Member
    {
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string PhoneNumber { get; set; }

        public MemberInfo(Member member, User user)
        {
            UserId = member.UserId;
            EventId = member.EventId;
            Name = user.Name;
            PhoneNumber = user.PhoneNumber;

        }
    }
}