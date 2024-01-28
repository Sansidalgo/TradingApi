using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class UserSubscriptionResponseDto
    {
        public int Id { get; set; }

        public int? TraderId { get; set; }

        public int? PlanId { get; set; }

        public DateTime StartDt { get; set; }

        public DateTime? EndDt { get; set; }

        public int SubscriptionStatusId { get; set; }
       

    }
}
