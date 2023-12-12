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
        public string Name { get; set; }
        public string Instrument { get; set; } = null!;

        public string? ExpiryDay { get; set; }

        public int? LotSize { get; set; }

        public short? CeSideEntryAt { get; set; }

        public short? PeSideEntryAt { get; set; }     

        public int TraderId { get; set; }
    }
}
