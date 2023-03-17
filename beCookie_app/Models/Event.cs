using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace beCookie_app.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public int? AdminId { get; set; }
        public string? Title { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? Schedule { get; set; }
        public string? Date { get; set; }
        public string? Adress { get; set; }
        public NpgsqlPoint? Location { get; set; }

        public virtual User? Admin { get; set; }
    }
}
