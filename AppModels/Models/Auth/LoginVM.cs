using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.Auth
{
    public sealed class LoginVM
    {
        [Required,DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
