using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class StrategyResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int TraderId { get; set; }

        public string? Description { get; set; }

        public DateTime? CreatedDt { get; set; }

        public DateTime? UpdatedDt { get; set; }

        public int? CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }
    }
}
