using System;
using System.Collections.Generic;

namespace beCookie_app.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Events = new HashSet<Event>();
            Likes = new HashSet<Like>();
            Posts = new HashSet<Post>();
        }
       
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public int? AvatarId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Points { get; set; }
        public int? Status { get; set; }

        public virtual Avatar? Avatar { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
