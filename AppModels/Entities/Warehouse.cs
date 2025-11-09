using AppModels.Common;
using System.ComponentModel.DataAnnotations;

namespace AppModels.Entities
{
    public class Warehouse : BaseEntity
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Location { get; set; }

        // ✅ General Info
        public string? ManagerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<MaterialType> materialTypes { get; set; } = new List<MaterialType>();
    }
}
