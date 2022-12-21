using System;
using System.Collections.Generic;

namespace beCookie_app.Models
{
    public partial class Like
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public int? UserLiked { get; set; }

        public virtual Post? Post { get; set; }
        public virtual User? User { get; set; }
    }
}
