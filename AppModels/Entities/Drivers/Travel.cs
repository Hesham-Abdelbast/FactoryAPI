using AppModels.Common;

namespace AppModels.Entities.Drivers
{
    public class Travel : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string StartLocation { get; set; } = null!;
        public string? Destination { get; set; }

        public double? DistanceKm { get; set; }

        public string? PlateNumber { get; set; }

    }
}
