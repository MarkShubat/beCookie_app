using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {       
        [HttpGet]
        [Route("GetPostById")]
        public IEnumerable<PostInfo> GetPostById(int id, int currentUserId)
        {   
            var context = new wypxrkenContext();
            var context1 = new wypxrkenContext();
            var users = context.Users;
            var posts = context1.Posts;
            var elem = posts.Where(post => post.Id == id).FirstOrDefault();
            var user = users.Where(user => user.Id == elem.UserId).FirstOrDefault();
            yield return new PostInfo(elem, user, currentUserId);
        }

        [HttpGet]
        [Route("GetPosts")]
        public IEnumerable<PostInfo> GetPosts(int currentUserId)
        {
            var context = new wypxrkenContext();
            var context1 = new wypxrkenContext();
            var users = context.Users;
            var posts = context1.Posts;
            foreach (var elem in posts)
            {
                var user = users.Where(user => user.Id == elem.UserId).FirstOrDefault();
                yield return new PostInfo(elem, user, currentUserId);
            }
        }

        [HttpGet]
        [Route("GetLikedPosts")]
        public IEnumerable<PostInfo> GetLikedPosts(int currentUserId)
        {
            var context = new wypxrkenContext();
            var context1 = new wypxrkenContext();
            var users = context.Users;
            var posts = context1.Posts;
            foreach(var elem in posts)
            {
                var user = users.Where(user => user.Id == elem.UserId).FirstOrDefault();
                var info = new PostInfo(elem, user, currentUserId);
                if (info.IsUserLiked) yield return info;
            }
        }

        [HttpPost]
        [Route("AddPost")]
        public IActionResult Add(PostData data)
        {
            var text = data.text;
            var userId = data.userId;

            var context = new wypxrkenContext();
            var post = new Post()
            {
                Text = text,
                UserId = userId
            };
            context.Posts.Add(post);           
            context.SaveChangesAsync();
            return Ok(post);
        }

        [HttpDelete]
        [Route("DeleteAll")]
        public IActionResult Delete()
        {
            var context = new wypxrkenContext();
            foreach (var elem in context.Posts)
            {
                context.Posts.Remove(elem);
            }
            context.SaveChangesAsync();
            return Ok("записи удалены");
        }
    }

    public class PostInfo
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? userAvatar { get; set; }
        public string? Date { get; set; }
        public string? Text { get; set; }
        public int likesCount { get; set; }
        public bool IsUserLiked { get; set; }

        public PostInfo(Post post, User user, int currentUserId)
        {
            Id = post.Id;
            UserId = post.UserId;
            UserName = user.Name;
            if(user.Avatar != null)
                userAvatar = user.Avatar.Url;
            Date = post.Date;
            Text = post.Text;
            var context = new wypxrkenContext();
            likesCount = context.Likes.Where(like => like.PostId == post.Id && like.UserLiked == 1).Count();
            var like = context.Likes.Where(like => like.PostId == post.Id && like.UserId == currentUserId).FirstOrDefault();
            if(like != null)
            {
                IsUserLiked = like.UserLiked == 1;
            }
        }
    }
    public class PostData
    {
        public string text { get; set; }
        public int userId { get; set; }
    }

}