using AppModels.Common;

namespace AppModels.Entities.Drivers
{
    public class Driver : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public decimal MoneyBalance { get; set; } = 0m;

        public ICollection<DriverExpense> Expenses { get; set; } = new List<DriverExpense>();

    }
}
