using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLU.Dtos
{
    public class PaymentsResponseDto
    {

        public int Id { get; set; }
        public string PlanName { get; set; }
        public string PlanTerm { get; set; }
        public int? TraderId { get; set; }

        public int? SubscriptionId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly PaymentDt { get; set; }

        public int? OfferId { get; set; }

        public bool? Status { get; set; }

        public string? TransactionId { get; set; }


    }
}
