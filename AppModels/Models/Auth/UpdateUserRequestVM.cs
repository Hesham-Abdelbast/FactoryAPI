
using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.Auth
{
    public sealed class UpdateUserRequestVM
    {
        [EmailAddress]
        public string NewEmail { get; set; }

        [MinLength(8)]
        public string NewPassword { get; set; }
    }
}
