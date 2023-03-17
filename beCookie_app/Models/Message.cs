using System;
using System.Collections.Generic;

namespace beCookie_app.Models
{
    public  class Message
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? EventId { get; set; }
        public string? Text { get; set; }
        public string? Date { get; set; }
    }
}
