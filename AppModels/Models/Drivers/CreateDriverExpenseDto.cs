using AppModels.Entities.Drivers;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppModels.Models.Drivers
{
    public sealed class CreateDriverExpenseDto
    {
        public Guid? Id { get; set; }
        public Guid DriverId { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}
