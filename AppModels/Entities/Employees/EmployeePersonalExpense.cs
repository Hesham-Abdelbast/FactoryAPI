using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities.Employees
{
    public class EmployeePersonalExpense : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; } // مصاريف شخصية

        [MaxLength(300)]
        public string? Note { get; set; }
    }
}
