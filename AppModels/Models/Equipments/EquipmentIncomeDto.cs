namespace AppModels.Models.Equipments
{
    public class EquipmentIncomeDto
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public string? EquipmentName { get; set; }
        public decimal Amount { get; set; }
        public string? RentalName { get; set; }
        public string? Note { get; set; }
    }
}
