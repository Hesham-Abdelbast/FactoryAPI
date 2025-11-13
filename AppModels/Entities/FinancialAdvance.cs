using AppModels.Common;
using AppModels.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities
{
    public class FinancialAdvance : BaseEntity
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public UserType userType { get; set; }
        [Required]
        public decimal TotalPaid { get; set; }





        public string? Notes { get; set; }

    }
}
