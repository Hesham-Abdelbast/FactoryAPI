using AppModels.Common;
using AppModels.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities.Equipments
{
    public class EquipmentExpense : BaseEntity
    {
        public Guid EquipmentId { get; set; }
        public Equipment Equipment { get; set; } = default!;

        public EquipmentExpenseType Type { get; set; } = EquipmentExpenseType.Other;

        public decimal Amount { get; set; } // مصروف: صيانة/وقود/زيت/إيجار/أخرى

        [MaxLength(300)]
        public string? Note { get; set; }
    }
}
