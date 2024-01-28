using BLU.Dtos;
using BLU.Enums;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace sansidalgo.Server.Controllers
{
    //[Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly PaymentsRepository repo;
        public PaymentsController(AlgoContext _context, PaymentsRepository _repo)
        {
            context = _context;
            repo = _repo;

        }
       
        [HttpGet("GetPaymentsByTraderId")]
        public async Task<DbStatus> GetPaymentsByTraderId()
        {
            var sid = User.Claims
               .Where(c => c.Type == ClaimTypes.Sid)
               .Select(c => c.Value)
               .SingleOrDefault();
            return await this.repo.GetPaymentsByTraderId(Convert.ToInt32(sid));
        }
        [HttpPost("AddPayments")]
        public async Task<ActionResult<DbStatus>> AddPayments(PaymentsRequestDto payment)
        {
            var sid = User.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value)
                .SingleOrDefault();

            payment.TraderId = Convert.ToInt32(sid);         
            return await repo.AddPayments(payment);


        }


    }
}
