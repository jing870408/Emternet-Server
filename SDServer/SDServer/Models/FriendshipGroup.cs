using System;
using System.Collections.Generic;

namespace SDServer.Models
{
    public partial class FriendshipGroup
    {
        public FriendshipGroup()
        {
            Friendship = new HashSet<Friendship>();
        }

        public string Account { get; set; }
        public string GroupId { get; set; }

        public virtual ICollection<Friendship> Friendship { get; set; }
    }
}
