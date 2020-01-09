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
    public class MembersController : ControllerBase
    {
        private readonly SDContext _context;

        public MembersController(SDContext context)
        {
            _context = context;
        }

        // GET: api/Members
        [HttpGet]
        public ActionResult<DateTime> GetMembers()
        {

            return DateTime.Now;
        }

        // GET: api/Members/5
        //驗證使用者
       /* [HttpGet("{account}")]
       public async Task<ActionResult<Members>> GetMembers(string account)
        {
            var members = await _context.Members.FindAsync(account);

            if (members == null)
            {
                return NotFound();
            }
            return members;
        }
        */

        // POST: api/Members
        //註冊新的使用者
        [HttpPost]
        public async Task<ActionResult<Boolean>> PostMembers(Members members)
        {
            _context.Members.Add(members);

            var friendshipgroup = new FriendshipGroup { Account = members.Account, GroupId = members.Email };
            var accountemo = new AccountEmo { Account = members.Account,Mood = "calm"};
            _context.FriendshipGroup.Add(friendshipgroup);
            _context.AccountEmo.Add(accountemo); 
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MembersExists(members.Account))
                {
                    return Conflict();
                }else 
                {
                    throw;
                }
            }                

            return true;
        }

        //帳號登入
        [HttpPost("{account}/{password}")]
        public async Task<ActionResult<string>> Login(String account,String password)
        {
            var members = await _context.Members.FindAsync(account);

            if (members.Password == password)
            {
                return "true";
            }
            else
            {
                return "false";
            }

        }

        private bool MembersExists(string id)
        {
            return _context.Members.Any(e => e.Account == id);
        }
    }
}