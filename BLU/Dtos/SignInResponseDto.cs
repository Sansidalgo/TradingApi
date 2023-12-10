using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class SignInResponseDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string EmailId { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;
   
        public string Role { get; set; } = null!;

        //[sridhar, 09-12-23]: The below properties will be used for local logic, not getting from database
        public string Token { get; set; } = null!;
    }
}
