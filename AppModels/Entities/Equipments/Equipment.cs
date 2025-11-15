using AppModels.Common;
using AppModels.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities.Equipments
{
    public class Equipment : BaseEntity
    {

        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        public EquipmentCategory Category { get; set; } = EquipmentCategory.Internal;

        // للمعدات الخارجية فقط: المالك
        public string OwnerPartner { get; set; }

        // للمعدات الخارجية: قيمة الإيجار (يمكن اعتبارها باليوم/الشهر حسب سياستك)
        public decimal? RentalValue { get; set; }

        public string? Notes { get; set; }
        // تنقلات
        public ICollection<EquipmentExpense> Expenses { get; set; } = new List<EquipmentExpense>();
        public ICollection<EquipmentIncome> Incomes { get; set; } = new List<EquipmentIncome>();
    }
}
