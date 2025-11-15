using AppModels.Common.Enums;

namespace AppModels.Models.Equipments
{
    public class EquipmentExpenseDto
    {
        public Guid Id { get; set; }
        public Guid EquipmentId { get; set; }
        public string? EquipmentName { get; set; }
        public EquipmentExpenseType Type { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
    }
}
