using System;

namespace MTA.Core.Application.Dtos
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Cost { get; set; }
        public DateTime DatePurchased { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
    }
}