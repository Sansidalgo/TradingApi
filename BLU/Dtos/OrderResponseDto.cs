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

        public int? Quantity { get; set; }

        public DateTime CreatedDt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? UpdatedDt { get; set; }

        public int? UpdatedBy { get; set; }

        public string? Asset { get; set; }

        public int TraderId { get; set; }

        public int? OrderSideId { get; set; }

        public int? SegmentId { get; set; }

        public int? EnvironmentId { get; set; }

        public int? OrderSourceId { get; set; }

        public decimal? BuyAt { get; set; }

        public decimal? SellAt { get; set; }

        public int? OrderSettingsId { get; set; }

        public int? StatusId { get; set; }

    }
}
