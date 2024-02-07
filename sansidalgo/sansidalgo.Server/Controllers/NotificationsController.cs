using BLU.Repositories;
using BLU.Enums;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;

namespace sansidalgo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly NotificationsRepository repo;
        private readonly IMapper mapper;
        public NotificationsController(AlgoContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            repo = new NotificationsRepository(_context);

        }

        [HttpGet("GetNotifications")]
        public async Task<DbStatus> GetNotifications()
        {
            var sid = User.Claims
               .Where(c => c.Type == ClaimTypes.Sid)
           .Select(c => c.Value)
               .SingleOrDefault();

            return await this.repo.GetNotifications(Convert.ToInt32(sid));
        }
    }
}
