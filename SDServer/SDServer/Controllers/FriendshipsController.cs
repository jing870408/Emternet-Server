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
   public class FriendshipsController : ControllerBase
    {
        private readonly SDContext _context;

        public FriendshipsController(SDContext context)
        {
            _context = context;
        }

        //尋找GroupId
        public string GetFriendshipGroup(string id)
        {
            var friendshipGroup =  _context.FriendshipGroup.Find(id);

            return friendshipGroup.GroupId;
        }

        // GET: api/Friendships/id
        //取得好友名單
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<String>>> GetFriendship(string id)
        {
            var Groupid = GetFriendshipGroup(id);
            var friendship = from i in _context.Friendship
                             where i.GroupId == Groupid
                             select  i.Account;

            if (!friendship.Any())
            {
                return NotFound("沒朋友");
            }

            var friendlist = friendship.ToListAsync();


            return await friendlist;
        }

        // POST: api/Friendships
        //新增好友
        [HttpPost("{account1}/{account2}")]
        public async Task<ActionResult<Boolean>> PostFriendship(string account1,string account2)
        {
            var AccountVaild = await _context.Members.FindAsync(account2);

            if (AccountVaild == null)
            {
                //無此人
                return NotFound();
            }

            var AGroup = GetFriendshipGroup(account1);
            var BGroup = GetFriendshipGroup(account2);
            
             
            var friendshipA = new Friendship { GroupId = AGroup,Account = account2};
            var friendshipB = new Friendship { GroupId = BGroup,Account = account1};

            var friendvalid = from o in _context.Friendship
                              where o.GroupId == AGroup && o.Account == account2
                              select o;

            if(friendvalid.Any())
            {
                return Conflict("好友已存在");
            }

            _context.Friendship.Add(friendshipA);
            _context.Friendship.Add(friendshipB);

            await _context.SaveChangesAsync();
            
            

            return true;
        }

        // DELETE: api/Friendships/5
        /*[HttpDelete("{id}")]
        public async Task<ActionResult<Friendship>> DeleteFriendship(string id)
        {
            var friendship = await _context.Friendship.FindAsync(id);
            if (friendship == null)
            {
                return NotFound();
            }

            _context.Friendship.Remove(friendship);
            await _context.SaveChangesAsync();

            return friendship;
        }*/
    }
}
