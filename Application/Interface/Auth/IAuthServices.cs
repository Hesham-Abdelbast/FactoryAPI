using AppModels.Models.Auth;
using Microsoft.AspNetCore.Identity.Data;

namespace Application.Interface.Auth
{
    public interface IAuthServices
    {
        Task<AuthResponseVM> RegisterAsync(RegistrationVM request);
        Task<AuthResponseVM> LoginAsync(LoginVM request);

        Task RequestPasswordResetAsync(string email);
        Task<AuthResponseVM> ConfirmEmailAsync(string userId, string token);
        Task ResetPasswordAsync(ResetPasswordRequest request);
        Task AssignRoleAsync(string userId, string roleName);
        Task UpdateUserAsync(string userId, UpdateUserRequestVM request);
        Task DeleteUserAsync(string userId);
        Task<UserProfileResponseVM> GetUserProfileAsync(string userId);
        Task LockUserAccountAsync(string userId, DateTimeOffset? lockoutEnd);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
    }
}
