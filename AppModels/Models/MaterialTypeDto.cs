namespace AppModels.Models
{
    public sealed class MaterialTypeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
