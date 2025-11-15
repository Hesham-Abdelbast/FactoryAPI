namespace AppModels.Models.Equipments
{
    public class EquipmentFinancialSummaryDto
    {
        public Guid EquipmentId { get; set; }
        public string? EquipmentName { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalIncomes { get; set; }
    }
}
