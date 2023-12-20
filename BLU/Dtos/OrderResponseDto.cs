using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class OrderResponseDto
    {
        public int Id { get; set; }

        public decimal? IndexPriceAt { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }     
        public string? Asset { get; set; }
        public string? OrderSideName { get; set; }

        public string? SegmentName { get; set; }
        public string? InstrumentName { get; set; }
        public string? EnvironmentName { get; set; }

        public string? OrderSourceName { get; set; }

       

        public string? StrategyName { get; set; }


    }
}
