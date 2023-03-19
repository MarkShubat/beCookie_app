using System;
using System.Collections.Generic;

namespace beCookie_app.Models
{
    public  class Message
    {
        public string? id { get; set; }
        public string? userId { get; set; }
        public string? userName { get; set; } 
        public string? eventId { get; set; }
        public string? text { get; set; }
        public string? date { get; set; }
        public bool? edited { get; set; }
    }
}
