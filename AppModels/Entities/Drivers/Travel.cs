using AppModels.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppModels.Entities.Drivers
{
    public class Travel : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? StartLocation { get; set; }
        public string? Destination { get; set; }
        public string? PlateNumber { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string? Notes { get; set; }

        public Guid DriverId { get; set; }
        [ForeignKey(nameof(DriverId))]
        public Driver Driver { get; set; }
    }
}
