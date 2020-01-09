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
    public class SongListFsController : ControllerBase
    {
        private readonly SDContext _context;

        public SongListFsController(SDContext context)
        {
            _context = context;
        }

        // GET: api/SongListFs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongListF>>> GetSongListF()
        {
            return await _context.SongListF.ToListAsync();
        }

        [HttpGet("{label}")]
        public ActionResult<string> GetSongList(string label)
        {
            Random ran = new Random();
            var songList = from i in _context.SongListF
                           where i.Label == label
                           select i;
            var List = songList.ToList();

            int idx = ran.Next(0, List.Count);

            if (songList == null)
            {
                return NotFound();
            }

            return List.ElementAt(idx).YtUrl;
        }
    }
}
