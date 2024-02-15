using AutoMapper;
using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DelegateController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly DelegateRepository repo;
        private readonly IMapper mapper;
        public DelegateController(AlgoContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            repo = new DelegateRepository(_context);

        }

        [HttpPost("AddDelegate")]
        public async Task<DbStatus> AddDelegate(int masterTraderId)
        {
            var sid = User.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value)
                .SingleOrDefault();
            TblDelegate delegateDetails = new TblDelegate();
            Console.WriteLine(sid+"::Master trader id:"+ masterTraderId.ToString());
            //delegateDetails.Id = Convert.ToInt32(sid);
            delegateDetails.TraderId = Convert.ToInt32(sid);
            delegateDetails.MasterTraderId = masterTraderId;
            delegateDetails.IsActive = false;
            delegateDetails.CreatedBy = Convert.ToInt32(sid);
            delegateDetails.UpdatedBy = Convert.ToInt32(sid);
            return await this.repo.AddDelegate(Convert.ToInt32(sid),delegateDetails, mapper);
        }

        [HttpGet("GetDelegates")]
        public async Task<DbStatus> GetDelegates()
        {
            var sid = User.Claims
               .Where(c => c.Type == ClaimTypes.Sid)
           .Select(c => c.Value)
               .SingleOrDefault();

            return await this.repo.GetDelegates(Convert.ToInt32(sid));
        }

        [HttpDelete("DeleteDelegate")]
        public async Task<DbStatus> DeleteDelegate(int Id)
        {
          //  var sid = User.Claims
          //    .Where(c => c.Type == ClaimTypes.Sid)
          //.Select(c => c.Value)
          //    .SingleOrDefault();
            return await this.repo.Delete(Id);
        }

        [HttpPost("UpdateDelegate")]
        public async Task<DbStatus> UpdateDelegate(int Id)
        {
           return await this.repo.UpdateDelegate(Id);
        }

    }
}
