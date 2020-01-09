using System;
using System.Collections.Generic;

namespace SDServer.Models
{
    public partial class ChatRoom
    {
        public string SendContent { get; set; }
        public DateTime Sendtime { get; set; }
        public string Sender { get; set; }
        public int Idx { get; set; }
        public int RoomId { get; set; }

        public virtual ChatRoomList Room { get; set; }
    }
}
