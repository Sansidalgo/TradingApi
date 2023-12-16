using BLU.Enums;
using BLU.Repositories;
using BLU.Repositories.Interfaces;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstrumentsController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly InstrumentsRepository repo;
        public InstrumentsController(AlgoContext _context)
        {
            context = _context;
            repo = new InstrumentsRepository(_context);

        }
        [HttpGet("GetInstruments")]
        public async Task<DbStatus> GetInstruments()
        {

            return await this.repo.GetInstruments();
        }
    }
}
