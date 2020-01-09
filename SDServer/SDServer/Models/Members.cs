using System;
using System.Collections.Generic;

namespace SDServer.Models
{
    public partial class Members
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
    }
}
