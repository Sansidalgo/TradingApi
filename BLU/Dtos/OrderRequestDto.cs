using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class OrderRequestDto
    {
        public int Id { get; set; }
        public DateTime CreatedDt { get; set; }
        public int TraderId { get; set; }

        public int? EnvironmentId { get; set; }

        public int? StatusId { get; set; }
    }
}
