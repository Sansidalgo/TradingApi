using BLU.Dtos;
using BLU.Repositories.Interfaces;
using BLU.Repositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BLU.Enums;
using NuGet.Common;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly AlgoContext _context;
    private readonly ILoginRepository repo;

    public LoginController(AlgoContext context)
    {
        _context = context;
        this.repo = new LoginRepository(_context);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] TraderDetailsRequestDto model)
    {
        // Validate user credentials (e.g., check against a database)
        var res = await IsValidUser(model);
        if (res.Status==1)
        {
            var token = GenerateToken(model.EmailId);
            return Ok(new { Token = token });
        }
        else if(!string.IsNullOrWhiteSpace(res.Message))
        {
            return Ok(new { Error = res.Message });
        }

        return Unauthorized();
    }
    // POST: api/TblTraderDetails
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    [HttpPost("SignUp")]
    public async Task<ActionResult<DbStatus>> SignUp(TraderDetailsRequestDto tblTraderDetail)
    {

        DbStatus res = await this.repo.SaveTraderDetails(tblTraderDetail);

        return res;
    }

    private async Task<DbStatus> IsValidUser(TraderDetailsRequestDto model)
    {
        
        return await this.repo.VerifyUser(model);
    }

    private string GenerateToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
            // Add more claims if needed
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("01B718E1348642199422B0D8DBC0A6BD"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "SansidAlgo",
            audience: "SansidAlgoAudience",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), // Token expiration time
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}

