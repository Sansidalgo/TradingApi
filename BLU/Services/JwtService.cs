using BLU.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BLU.Services
{
    public class JwtService
    {
        private readonly string secretKey;
       

        public JwtService(string secretKey)
        {
            this.secretKey = secretKey;
           
        }
        public string GenerateToken(SignInResponseDto responseDto)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenkey = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                         new Claim(ClaimTypes.Name, responseDto.Name.Trim()),
            new Claim(ClaimTypes.Email, responseDto.EmailId),
            new Claim(ClaimTypes.Role, responseDto.Role.Trim()),
                    new Claim(ClaimTypes.Sid,Convert.ToString(responseDto.Id)),

                    }
                ),
                Expires = DateTime.Now.AddHours(9),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenkey), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            string finaltoken = tokenhandler.WriteToken(token);

            return finaltoken;

        }
        //public ClaimsPrincipal DecodeToken(string token)
        //{
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var validationParameters = GetValidationParameters();

        //        // Validate the token and retrieve the claims
        //        var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

        //        // You can access user details from the ClaimsPrincipal
        //        // For example, to get the user ID:
        //        var userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //        Console.WriteLine("User ID: " + userId);

        //        return claimsPrincipal;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error decoding token: " + ex.Message);
        //        return null;
        //    }
        //}

        //private TokenValidationParameters GetValidationParameters()
        //{
        //    return new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = false,
        //        RequireSignedTokens = false,

               

        //        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey)),

        //    };
        //}
    }

}


