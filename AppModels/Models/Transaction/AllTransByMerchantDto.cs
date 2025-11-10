namespace AppModels.Models.Transaction
{
    public sealed class AllTransByMerchantDto
    {
        public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
        public decimal TotalMoneyProcessed { get; set; }
        public decimal TotalMoneypay { get; set; }
        public decimal TotalImpurities { get; set; }
        public decimal TotalWight { get; set; }
    }
}
