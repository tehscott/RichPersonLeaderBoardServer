using System;

namespace Server.Models
{
    public class CreatePaymentRequest
    {
        public string GoogleId { get; set; }
        public Decimal Amount { get; set; }
    }
}