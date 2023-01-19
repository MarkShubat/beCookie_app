using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FaqController : ControllerBase
    {       
        [HttpGet]
        [Route("GetFaqById")]
        public Faq GetFaqById(int id)
        {   
            var context = new wypxrkenContext();
            return context.Faqs.Where(faq => faq.Id == id).FirstOrDefault();
        }

        [HttpGet]
        [Route("GetFaqs")]
        public IEnumerable<Faq> GetFaqs()
        {
            var context = new wypxrkenContext();
            return context.Faqs;
        }

        [HttpGet]
        [Route("GetFaqByType")]
        public IEnumerable<Faq> GetByType(int type)
        {
            var context = new wypxrkenContext();
            return context.Faqs.Where(faq => faq.Type == type);
        }

        [HttpPost]
        [Route("AddFaq")]
        public IActionResult Add(string text, string tytle, int type, string image_url)
        {
            var context = new wypxrkenContext();
            var faq = new Faq
            {
                Type = type,
                Tytle = tytle,
                Image_url = image_url,
                Text = text
            };
            context.Faqs.Add(faq);
            context.SaveChangesAsync();
            return Ok("Совет добавлен");
        }

        [HttpDelete]
        [Route("DeleteAll")]
        public IActionResult Delete()
        {
            var context = new wypxrkenContext();
            foreach (var elem in context.Faqs)
            {
                context.Faqs.Remove(elem);
            }
            context.SaveChangesAsync();
            return Ok("записи удалены");
        }
    }
}