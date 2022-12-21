using System;
using System.Collections.Generic;
using beCookie_app.DbContexts;
using beCookie_app.Methods;

namespace beCookie_app.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            Date = DateTimeConverter.GetDateTimeString();
            var context = new wypxrkenContext();
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Date { get; set; }
        public string? Text { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
    }
}
