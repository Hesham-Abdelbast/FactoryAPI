using System.ComponentModel.DataAnnotations;

namespace AppModels.Models.MerchantMangement
{
    public sealed class MerchantDto
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// Full name of the merchant
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Contact phone number of the merchant
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// Email address of the merchant
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Physical address of the merchant
        /// </summary>
        [StringLength(200)]
        public string? Address { get; set; }
    }
}
