using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminPageController : ControllerBase
    {
        [Route("Index")]
        [HttpGet]
        public ContentResult Index()
        {
            StreamReader sr = new StreamReader("C:\\Users\\Марк\\source\\repos\\beCookie_app\\beCookie_app\\Views\\AdminPage\\AdminPage.cshtml");
            var line = sr.ReadLine();
            var sb = new StringBuilder();

            while (line != null)
            {
                sb.Append(line);
                line = sr.ReadLine();
            }
            sr.Close();
            return new ContentResult
            {
                ContentType = "text/html",

                Content = sb.ToString()
            };
        }
    }
}