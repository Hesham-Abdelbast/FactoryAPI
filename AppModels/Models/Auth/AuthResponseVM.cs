namespace AppModels.Models.Auth
{
    public sealed class AuthResponseVM
    {
        public string? Message { get; set; } = null!;
        public string? Token { get; set; } = null!;
        public bool IsAuthenticated { get; set; } 
        public string? UserId { get; set; } = null!;
        public string? UserName { get; set; } = null!;
        public string? Email { get; set; } = null!;
        public DateTime ExpiresOn { get; set; }
        public List<string>? Roles { get; set; }
    }
}
