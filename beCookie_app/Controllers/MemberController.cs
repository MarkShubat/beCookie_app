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
        [Route("GetMemberById")]
        public Member GetMemberById(int id)
        {   
            var context = new wypxrkenContext();
            return context.Members.Where(member => member.UserId == id).FirstOrDefault();
        }

        [HttpGet]
        [Route("GetEventMembers")]
        public IEnumerable<Member> GetEventMembers(int id)
        {
            var context = new wypxrkenContext();
            return context.Members.Where(member => member.EventId == id);
        }
    }
}