using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using sansidalgo.Models;

namespace sansidalgo
{

    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
      

        public RefreshTokenGenerator()
        {
           
        }
        public string GenerateToken(string username)
        {
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                string RefreshToken = Convert.ToBase64String(randomnumber);

               

                return RefreshToken;
            }
        }
    }
}
