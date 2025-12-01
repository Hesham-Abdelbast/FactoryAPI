using AppModels.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.SystemInventory
{
    public sealed class TrnxReportRequestDto
    {
        [Required]
        public DateTime From { get; set; }
        [Required]
        public DateTime To { get; set; }
        public Guid? WarehouseId { get; set; } = null;
        public MaterialCategory? MaterialCategory { get; set; } = null;
    }
}
