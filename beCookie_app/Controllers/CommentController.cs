using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {       
        [HttpGet]
        [Route("GetUserComments")]
        public IEnumerable<CommentInfo> GetUserComments(int id)
        {
            var context = new wypxrkenContext();
            var context1 = new wypxrkenContext();
            var users = context1.Users;
            foreach (var elem in context.Comments.Where(comment => comment.UserId == id))
            {
                var user = users.Where(user => user.Id == elem.UserId).FirstOrDefault();
                yield return new CommentInfo(elem, user);
            }
        }

        [HttpGet]
        [Route("GetPostComments")]
        public IEnumerable<CommentInfo> GetPostComments(int id)
        {
            var context = new wypxrkenContext();
            var context1 = new wypxrkenContext();
            var users = context1.Users;
            foreach( var elem in context.Comments.Where(comment => comment.PostId == id))
            {
                var user = users.Where(user => user.Id == elem.UserId).FirstOrDefault();
                yield return new CommentInfo(elem, user);
            }
        }

        [HttpPost]
        [Route("AddComment")]
        public IActionResult Add(CommentData data)
        {
            var text = data.text;
            var postId = data.postId;
            var userId = data.userId;

            var context = new wypxrkenContext();
            var comment = new Comment
            {
                Text = text,
                PostId = postId,
                UserId = userId,
            };
            context.Comments.Add(comment);
            var user = context.Users.Where(user => user.Id == userId).FirstOrDefault();
            context.SaveChangesAsync();
            return Ok(new CommentInfo(comment, user));
        }

        [HttpDelete]
        [Route("DeleteAll")]
        public IActionResult Delete()
        {
            var context = new wypxrkenContext();
            foreach (var elem in context.Comments)
            {
                context.Comments.Remove(elem);
            }
            context.SaveChangesAsync();
            return Ok("записи удалены");
        }

        public class CommentInfo
        {
            public int Id { get; set; }
            public int? PostId { get; set; }
            public int? UserId { get; set; }
            public string? UserName { get; set; }
            public string? userAvatar { get; set; }
            public string? Date { get; set; }
            public string? Text { get; set; }

            public CommentInfo(Comment comment, User user)
            {
                Id = comment.Id;
                PostId = comment.PostId;
                UserId = comment.UserId;
                UserName = user.Name;
                if (user.Avatar != null)
                    userAvatar = user.Avatar.Url;
                Date = comment.Date;
                Text = comment.Text;
            }
        }
    }
    public class CommentData
    {
        public string text { get; set; }
        public int userId { get; set; }
        public int postId { get; set; }
    }
}