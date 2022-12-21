using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace beCookie_app.Models
{
    public partial class Point
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Schedule { get; set; }
        public string? Date { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Types { get; set; }
        public string? Adress { get; set; }
        public NpgsqlPoint? Location { get; set; }
    }
}
