using System;
using System.Collections.Generic;

namespace SDServer.Models
{
    public partial class ChatRoomList
    {
        public ChatRoomList()
        {
            ChatRoom = new HashSet<ChatRoom>();
        }

        public string Account { get; set; }
        public string Person { get; set; }
        public int Idx { get; set; }

        public virtual ICollection<ChatRoom> ChatRoom { get; set; }
    }
}
