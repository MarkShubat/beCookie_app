using System;
using System.Collections.Generic;

namespace beCookie_app.Models
{
    public partial class Member
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? EventId { get; set; }
    }
}
