using BLU.Repositories.Interfaces;
using BLU.Repositories;
using BLU.Services;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLU.Dtos;
using BLU.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using AutoMapper;

namespace sansidalgo.Server.Controllers
{
    [Authorize(Roles = "user")]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoonyaCredentialsController : ControllerBase
    {
        private readonly AlgoContext context;
        private readonly ShoonyaCredentialsRepository repo;
        private readonly IMapper mapper;
        public ShoonyaCredentialsController(AlgoContext _context, IMapper _mapper)
        {
            context = _context;
            mapper = _mapper;
            repo = new ShoonyaCredentialsRepository(_context);
           

        }
        [HttpPost("Add")]
        public async Task<ActionResult<DbStatus>> Add(TblShoonyaCredential credential)
        {
            var sid = User.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
                .Select(c => c.Value)
                .SingleOrDefault();

            credential.TraderId = Convert.ToInt32(sid);
            credential.CreatedBy = credential.TraderId;
            credential.CreatedDt = DateTime.UtcNow;
            return await this.repo.Add(credential);


        }

        [HttpGet("GetCredentials")]
        public async Task<DbStatus> GetCredentials()
        {
            var sid = User.Claims
                .Where(c => c.Type == ClaimTypes.Sid)
            .Select(c => c.Value)
                .SingleOrDefault();


            return await this.repo.GetCredentials(Convert.ToInt32(sid));
        }
    }


}
