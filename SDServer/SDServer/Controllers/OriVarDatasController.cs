using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    public class OriVarDatasController : ControllerBase
    {
        private readonly SDContext _context;
       
        
        public OriVarDatasController(SDContext context)
        {
            _context = context;
        }
        // POST: api/OriVarDatas
        [HttpPost]
        public async Task<ActionResult<Boolean>> PostOriVarData(OriVarData oriVarData)
        {
            //取得手機回傳變數 
            _context.OriVarData.Add(oriVarData);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
