using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics.Tracing;

namespace beCookie_app.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(ToRegistration userToReg)
        {
            var name = userToReg.name;
            var email = userToReg.email;
            var phoneNumber = userToReg.phoneNumber; 

            if (name.Length >= 100) return BadRequest("Слишком длинный никнейм");
            else if (!isEmailValid(email)) return BadRequest("Неверный формат почты или пользователь с такой почтой уже зарегистрирован");
            else if (!isNumberValid(phoneNumber)) return BadRequest("Неверный формат номера телефона или пользователь с таким телефоном уже зарегистрирован");
            else
            {
                var user = new User
                {
                    Email = email,
                    Name = name,
                    PhoneNumber = phoneNumber
                };
                var context = new wypxrkenContext();
                context.Users.Add(user);
                context.SaveChangesAsync();
                return Ok(user);
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string email)
        {
            var context = new wypxrkenContext();
            if (context.Users.Where(user => user.Email == email).Count() == 0) return BadRequest("Нет пользователя с такой почтой");
            var user = context.Users.Where(user => user.Email == email).FirstOrDefault();
            return Ok(user);
        }

        [HttpPost]
        [Route("Verify")]
        public IActionResult Verify(string code, string email)
        {
            var item = Data.verifCode.Where(item => item.Item2 == email).LastOrDefault();
            if (code == item.Item1)
            {
                return Ok("Успешный вход");
            }
            else return BadRequest("Неверный код или указана другая почта");
        }
        bool isEmailValid(string email)
        {
            var context = new wypxrkenContext();
            if (context.Users.Where(user => user.Email == email).Count() > 0) return false;
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";
            Match isMatch = Regex.Match(email, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        bool isNumberValid(string number)
        {
            var context = new wypxrkenContext();
            if (context.Users.Where(user => user.PhoneNumber == number).Count() > 0) return false;
            string pattern = @"((\+38|8|\+3|\+ )[ ]?)?([(]?\d{3}[)]?[\- ]?)?(\d[ -]?){6,14}";
            Match isMatch = Regex.Match(number, pattern, RegexOptions.IgnoreCase);
            return isMatch.Success;
        }

        [HttpPost]
        [Route("GenerateAndSendVerifCode")]
        public void GenerateAndSendVerifCode(string email)
        {
            var sb = new StringBuilder();
            var rn = new Random();
            for (var i = 0; i < 6; i++)
            {
                sb.Append(rn.Next(0, 9));
            }
            var code = sb.ToString();
            Data.verifCode.Add((code, email));

            MailAddress from = new MailAddress("marckshubat@yandex.ru", "admin");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Код подтверждения";
            var s = code;
            m.Body = s;
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            smtp.Credentials = new NetworkCredential("marckshubat@yandex.ru", "Mark022402");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
    public class ToRegistration
    {
        public string email { get; set; }
        public string name { get; set; }
        public string phoneNumber { get; set; }
    }
}

