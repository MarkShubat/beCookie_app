using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;

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

        [HttpPost]
        [Route("ToDo")]
        public IActionResult ToDo()
        {
            var context = new wypxrkenContext();
            foreach(var elem in context.Users)
            {
                elem.Status = 0;
                elem.Points = 0;
            }
            context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Route("AddPoints")]
        public IActionResult AddPoints(int userId, int count)
        {
            var context = new wypxrkenContext();
            var user = context.Users.Where(user => user.Id == userId).FirstOrDefault();
            user.Points += count;
            context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("SetStatus")]
        public IActionResult SetStatus(int userId)
        {
            var context = new wypxrkenContext();
            var user = context.Users.Where(user => user.Id == userId).FirstOrDefault();
            user.Status = 1;
            context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("AskForStatus")]
        public void AskForStatus(int currentUserId, string text)
        {
            var context = new wypxrkenContext();
            var user = context.Users.Where(user => user.Id == currentUserId).FirstOrDefault();
            MailAddress from = new MailAddress("marckshubat@yandex.ru", "admin");
            MailAddress to = new MailAddress("markshubat@gmail.com");
            MailMessage m = new MailMessage(from, to);
            var sb = new StringBuilder("ѕользователь с идентификатором ");
            sb.Append(currentUserId);
            sb.Append(" запрашивает получение статуса редактора контента");
            m.Subject = sb.ToString();
            var s = text;
            m.Body = s;
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            smtp.Credentials = new NetworkCredential("marckshubat@yandex.ru", "Mark022402");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}