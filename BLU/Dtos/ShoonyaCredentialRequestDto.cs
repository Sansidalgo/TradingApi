using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class ShoonyaCredentialRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TraderId { get; set; }

        public string? Uid { get; set; }

        public string? Password { get; set; }

        public string? AuthSecreteKey { get; set; }

        public string? Imei { get; set; }

        public string? Vc { get; set; }

        public string? ApiKey { get; set; }

        public bool? IsActive { get; set; }
    }
}
