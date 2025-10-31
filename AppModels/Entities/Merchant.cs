
using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities
{
    /// <summary>
    /// Represents a merchant (owner) who engages in transactions with the factory
    /// </summary>
    public class Merchant : BaseEntity
    {

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
        public string Phone { get; set; }

        /// <summary>
        /// Email address of the merchant
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Physical address of the merchant
        /// </summary>
        [StringLength(200)]
        public string Address { get; set; }

        /// <summary>
        /// Navigation property for transactions associated with this merchant
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
