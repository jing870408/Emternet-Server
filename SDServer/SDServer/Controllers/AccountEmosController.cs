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
    public class AccountEmosController : ControllerBase
    {
        private readonly SDContext _context;

        public AccountEmosController(SDContext context)
        {
            _context = context;
        }

        // GET: api/AccountEmos
        //測試
        [HttpGet("{id}")]
        public async Task<ActionResult<String>> GetAccountEmo(String id)
        {
            var accountemo = await _context.AccountEmo.FindAsync(id);

            if (accountemo == null)
            {
                return NotFound();
            }

            return accountemo.Mood;

        }

        [HttpPost("{id}/{emo}")]
        public async Task<ActionResult<IEnumerable<String>>> PostAccountEmo(String id, String emo)
        {

            var friendshipGroup = _context.FriendshipGroup.Find(id);

            var s = from q in _context.Friendship
                    where q.GroupId == friendshipGroup.GroupId
                    select q;
            List<String> list = new List<String>();
            list.Add("");
            foreach (var i in s.ToList())
            {
                var t = from p in _context.AccountEmo
                        where p.Account == i.Account && p.Mood == emo
                        select p.Account;
                if (t.Any()) {
                    list.Add(t.FirstOrDefault());
                }
            }
            return list;
        }



    }
}
