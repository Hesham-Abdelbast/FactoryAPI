using AppModels.Common;

namespace AppModels.Entities.Drivers
{
    public class Driver : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? LicenseNumber { get; set; }
        public DateTime LicenseExpiry { get; set; }
    }
}
