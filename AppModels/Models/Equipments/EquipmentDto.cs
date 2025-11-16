using AppModels.Common.Enums;

namespace AppModels.Models.Equipments
{
    public class EquipmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public EquipmentCategory Category { get; set; }
        public string? OwnerPartner { get; set; }
        public decimal? RentalValue { get; set; }

        public string? Notes { get; set; }
    }
}