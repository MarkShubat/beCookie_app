using beCookie_app.DbContexts;
using beCookie_app.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace beCookie_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LikeController : ControllerBase
    {       
        [HttpGet]
        [Route("GetLikesById")]
        public IEnumerable<Like> GetLikesById(int id)
        {   
            var context = new wypxrkenContext();
            return context.Likes.Where(like => like.UserId == id);
        }

        [HttpGet]
        [Route("GetPostLikes")]
        public IEnumerable<Like> GetPostLikes(int id)
        {
            var context = new wypxrkenContext();
            return context.Likes.Where(like => like.PostId == id);
        }

        [HttpPost]
        [Route("LikeOrUnlike")]
        public IActionResult LikeOrUnlike(int postid, int currentUserId)
        {
            var context = new wypxrkenContext();
            var likes = context.Likes.Where(like => like.UserId == currentUserId && like.PostId == postid);
            if (likes.Count() == 0)
            {
                var like = new Like
                {
                    UserId = currentUserId,
                    PostId = postid,
                    UserLiked = 1
                };
                context.Likes.Add(like);
                context.SaveChanges();
            }
            else
            {
                var like = likes.FirstOrDefault();
                if(like.UserLiked == 1)
                {
                    like.UserLiked = 0;
                    context.SaveChanges();
                }
                else
                {
                    like.UserLiked = 1;
                    context.SaveChanges();
                }
            }
            return Ok();
        }

    }
}