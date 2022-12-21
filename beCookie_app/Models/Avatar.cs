using System;
using System.Collections.Generic;

namespace beCookie_app.Models
{
    public partial class Avatar
    {
        public Avatar()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Url { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
