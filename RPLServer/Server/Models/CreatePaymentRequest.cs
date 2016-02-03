using System;

namespace Server.Models
{
    public class CreatePaymentRequest
    {
        public int PersonId { get; set; }
        public Decimal Amount { get; set; }
    }
}