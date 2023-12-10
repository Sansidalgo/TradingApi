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
using static System.Runtime.InteropServices.JavaScript.JSType;
using BLU.Services;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly AlgoContext _context;
    private readonly ILoginRepository repo;
    private readonly JwtService jwtService;
    public LoginController(AlgoContext context, JwtService jwtService)
    {
        _context = context;
        this.repo = new LoginRepository(_context);
        this.jwtService = jwtService;
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignInRequestDto model)
    {
        // Validate user credentials (e.g., check against a database)
        var res = await this.repo.SignIn(model);
        if (res.Status==1)
        {
            var token =jwtService.GenerateToken(res?.Result as SignInResponseDto);
         ( res?.Result as SignInResponseDto).Token = token;          
           
        }

        return Ok(res);

    }
    // POST: api/TblTraderDetails
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

    [HttpPost("SignUp")]
    public async Task<ActionResult<DbStatus>> SignUp(SignupRequestDto tblTraderDetail)
    {

        DbStatus res = await this.repo.SignUp(tblTraderDetail);

     
        return res;
    }

    

   
}

