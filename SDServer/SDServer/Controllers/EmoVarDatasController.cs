using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class EmoVarDatasController : ControllerBase
    {
        private readonly SDContext _context;


        public EmoVarDatasController(SDContext context)
        {
            _context = context;
        }

        // GET: api/EmoVarDatas
        //testing
        [HttpGet]
        public ActionResult<string> GetEmoVarData()
        {
            var emoVarData = from em in _context.EmoVarData
                             where em.Account == "Alexlin"
                             select em;

            return "" + emoVarData.FirstOrDefault().Date.ToString("yyyyMMdd");
        }

        // GET: api/EmoVarDatas/5
        [HttpPost("{account}/{date}")]
        public string PostEmoVarData(String account, String date)
        {
            var emoVarData = from em in _context.EmoVarData
                             where em.Account == account && em.Date.ToString("yyyyMMdd") == date
                             select em.評分;

            if (!emoVarData.Any())
            {
                return "沒有數據";
            }

            return emoVarData.FirstOrDefault();
        }
    }
}
