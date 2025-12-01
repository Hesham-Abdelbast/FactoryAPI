namespace AppModels.Models.Drivers
{
    public sealed class CreateDriverDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? LicenseNumber { get; set; }
    }
}
