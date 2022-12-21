using System;
using System.Collections.Generic;

namespace beCookie_app.Models
{
    public partial class Faq
    {
        public int Id { get; set; }
        public int? Type { get; set; }
        public string? Text { get; set; }
        public string? Tytle { get; set; }
    }
}
