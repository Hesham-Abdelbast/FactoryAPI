namespace AppModels.Models.Store
{
    public sealed class ExpenseWareSumResponse
    {
        public decimal TotalExpense { get; set; }
        public int TotalRecords { get; set; }
        public List<WarehouseExpenseDto>? Details { get; set; }
    }
}
