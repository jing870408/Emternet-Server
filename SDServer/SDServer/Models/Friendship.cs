using System;
using System.Collections.Generic;

namespace SDServer.Models
{
    public partial class Friendship
    {
        public string GroupId { get; set; }
        public string Mood { get; set; }
        public string Account { get; set; }
        public int Idx { get; set; }

        public virtual FriendshipGroup Group { get; set; }
    }
}
