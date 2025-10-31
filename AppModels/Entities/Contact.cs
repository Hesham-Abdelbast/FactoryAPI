using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities
{
    public class Contact:BaseEntity
    {

        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Phone { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100)]
        public string? Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Website { get; set; } = string.Empty;
    }
}