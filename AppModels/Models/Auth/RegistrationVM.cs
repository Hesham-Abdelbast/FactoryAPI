using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.Auth
{
    public class RegistrationVM
    {
        [Required]
        public string? FirstName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        public string? LastName { get; set; }
        public string? Address { get; set; }

        [Required]
        public string? Password { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; } = DateTime.UtcNow;

        public List<string>? Roles { get; set; }
    }
}
