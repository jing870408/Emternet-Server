using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SDServer.Models;


namespace SDServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomsController : ControllerBase
    {
        private readonly SDContext _context;
       
        public ChatRoomsController(SDContext context)
        {
            _context = context;
        }

        // POST: api/ChatRooms
        //輸入訊息後傳送
        [HttpPost]
        public async Task<ActionResult<Boolean>> PostChatRoom(ChatRoom chatRoom)
        {
            _context.ChatRoom.Add(chatRoom);
            await _context.SaveChangesAsync();

            //傳送成功
            return true;
        }

        //取得聊天紀錄
        [HttpPost("{account}/{person}")]
        public async Task<ActionResult<IEnumerable<ChatRoom>>> PostChatRecord(string account, string person)
        {
            var roomid = from room in _context.ChatRoomList
                         where (room.Account == account && room.Person == person)
                                  || (room.Account == person && room.Person == account)
                         select room;

            var record = from chat in _context.ChatRoom
                         where chat.RoomId == roomid.FirstOrDefault().Idx
                         select chat;

            return await record.ToListAsync();
        }
    }
}
