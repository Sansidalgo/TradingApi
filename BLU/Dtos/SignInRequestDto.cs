using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class SignInRequestDto
    {
        public string EmailId { get; set; } = null!;

        public string PhoneNo { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
