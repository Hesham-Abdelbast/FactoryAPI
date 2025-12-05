namespace AppModels.Models.Transaction
{
    public sealed class InvoiceLstDto
    {
        public decimal TotalAmount { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public List<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
    }
}
