using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.Drivers
{
    public sealed class CreateTravelDto
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartLocation { get; set; } = null!;
        public string? Destination { get; set; }
        public string? PlateNumber { get; set; }
        public decimal Amount { get; set; }
        public string? Notes { get; set; }
    }
}
