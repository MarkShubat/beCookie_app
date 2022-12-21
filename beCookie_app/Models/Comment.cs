using System;
using System.Collections.Generic;
using beCookie_app.Methods;

namespace beCookie_app.Models
{
    public partial class Comment
    {
        public Comment()
        {
            Date = DateTimeConverter.GetDateTimeString();
        }
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public string? Text { get; set; }
        public string? Date { get; set; }

        public virtual Post? Post { get; set; }
        public virtual User? User { get; set; }
    }
}
