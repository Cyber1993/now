﻿namespace Core.Models.PromoCode
{
    public class EditPromoCodeDto
    {
        public string Code { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? RemainingUses { get; set; }
    }
}
