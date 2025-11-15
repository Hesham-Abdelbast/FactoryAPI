using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities.Equipments
{
    public class EquipmentIncome : BaseEntity
    {
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; } = default!;
        public decimal Amount { get; set; } // وارد من التأجير

        public string? RentalName{ get; set; }

        [MaxLength(300)]
        public string? Note { get; set; }
    }
}
