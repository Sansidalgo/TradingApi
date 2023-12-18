using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class OptionsSettingRequestDto
    {
        public int Id { get; set; }

        public int InstrumentId { get; set; }

       

        public short? CeSideEntryAt { get; set; }

        public short? PeSideEntryAt { get; set; }

        public int? StrategyId { get; set; }

        public int TraderId { get; set; }

        public DateTime? CreatedDt { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? UpdatedDt { get; set; }

        public int? UpdatedBy { get; set; }

        public string? Exchange { get; set; }

      

        public string? Name { get; set; }

        public string? StartTime { get; set; }

        public string? EndTime { get; set; }

        public decimal? PlayCapital { get; set; }

        public decimal? PlayQuantity { get; set; }

        public int? StopLoss { get; set; }

        public int? Target { get; set; }

        public int? TrailingStopLoss { get; set; }

        public int? TrailingTarget { get; set; }

    }
}
