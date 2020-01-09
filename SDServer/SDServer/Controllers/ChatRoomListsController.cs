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
    public class ChatRoomListsController : ControllerBase
    {
        private readonly SDContext _context;

        public ChatRoomListsController(SDContext context)
        {
            _context = context;
        }

        // POST: api/ChatRoomLists
        //點擊好友選擇聊天後回傳群組
        [HttpPost("{account}/{person}")]
        public async Task<ActionResult<int>> PostChatRoomList(string account,string person)
        {
            //查看群組是否存在
            var roomvalid = from room in _context.ChatRoomList
                            where (room.Account == account && room.Person == person)
                                  || (room.Account == person && room.Person == account)
                            select room;
            
                

            if (roomvalid.Any())
            {
                int i = roomvalid.FirstOrDefault().Idx;
                return i;
            }

            var newroom = new ChatRoomList { Account = account, Person = person };

            _context.ChatRoomList.Add(newroom);
            await _context.SaveChangesAsync();

            var roomvalid2 = from room in _context.ChatRoomList
                            where (room.Account == account && room.Person == person)
                                  || (room.Account == person && room.Person == account)
                            select room;

            int ii = roomvalid2.FirstOrDefault().Idx;


            return ii;

        }
    }
}
