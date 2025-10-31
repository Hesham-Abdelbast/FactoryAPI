namespace AppModels.Models.Auth
{
    public sealed class UserProfileResponseVM
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
