using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.Models;
using BLU.Repositories.Interfaces;
using BLU.Dtos;
using BLU.Repositories;
using BLU.Enums;
using Microsoft.AspNetCore.Http.HttpResults;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TraderDetailsController : ControllerBase
    {
        private readonly AlgoContext _context;
        private readonly ITraderDetailsRepository repo;

        public TraderDetailsController(AlgoContext context)
        {
            _context = context;
            this.repo = new TraderDetailsRepository(_context);
        }
       
        // GET: api/TblTraderDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblTraderDetail>>> GetTblTraderDetails()
        {
            return await _context.TblTraderDetails.ToListAsync();
        }

        // GET: api/TblTraderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TblTraderDetail>> GetTblTraderDetail(int id)
        {
            var tblTraderDetail = await _context.TblTraderDetails.FindAsync(id);

            if (tblTraderDetail == null)
            {
                return NotFound();
            }

            return tblTraderDetail;
        }

        //// PUT: api/TblTraderDetails/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTblTraderDetail(int id, TblTraderDetail tblTraderDetail)
        //{
        //    if (id != tblTraderDetail.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(tblTraderDetail).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TblTraderDetailExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}



        //// DELETE: api/TblTraderDetails/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTblTraderDetail(int id)
        //{
        //    var tblTraderDetail = await _context.TblTraderDetails.FindAsync(id);
        //    if (tblTraderDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.TblTraderDetails.Remove(tblTraderDetail);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool TblTraderDetailExists(int id)
        //{
        //    return _context.TblTraderDetails.Any(e => e.Id == id);
        //}
    }
}
