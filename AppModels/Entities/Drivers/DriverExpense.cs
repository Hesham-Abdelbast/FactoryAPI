using AppModels.Common;
using AppModels.Common.Enums;

namespace AppModels.Entities.Drivers
{
    public class DriverExpense : BaseEntity
    {
        public Guid? TravelId { get; set; }
        public Travel? Travel { get; set; }

        public Guid? DriverId { get; set; }
        public Driver? Driver { get; set; }

        public DriverExpenseType ExpenseType { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }

        public DateTime ExpenseDate { get; set; }
    }
}
