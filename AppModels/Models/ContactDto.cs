using System.ComponentModel.DataAnnotations;

namespace AppModels.Models
{
    public class ContactDto
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = "اسم الشركة مطلوب")]
        [StringLength(100, ErrorMessage = "اسم الشركة لا يمكن أن يتجاوز 100 حرف")]
        public string CompanyName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;

        public string? Phone { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;

        public string? Website { get; set; } = string.Empty;
    }
}
