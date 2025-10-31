
using AutoMapper;

using Application.Interface.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.Data;
using DAL;
using AppModels.Models.Auth;

namespace Application.Implementation.Auth
{
    public class AuthServices : IAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;

        public AuthServices(UserManager<ApplicationUser> userManager, IConfiguration configuration, IMapper mapper,ITokenServices tokenServices)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _tokenServices = tokenServices;
        }
        public Task AssignRoleAsync(string userId, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<AuthResponseVM> ConfirmEmailAsync(string userId, string token)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<UserProfileResponseVM> GetUserProfileAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetUserRolesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task LockUserAccountAsync(string userId, DateTimeOffset? lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponseVM> LoginAsync(LoginVM request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if(user == null)
                {
                    throw new Exception("Invalid user name or password"); // i will not say "User not found" because it can be a security risk
                }
                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!result)
                {
                    throw new Exception("Invalid user name or password");
                }

                var roles = await _userManager.GetRolesAsync(user);

                var token = _tokenServices.CreateJWTToken(user,roles?.ToList());
                return new AuthResponseVM
                {
                    Message = "Login successful",
                    IsAuthenticated = true,
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles?.ToList(),
                    Token = token,
                    ExpiresOn = DateTime.UtcNow.AddHours(1) //  token expires in 1 hour
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}");
            }

        }

        public async Task<AuthResponseVM> RegisterAsync(RegistrationVM model)
        {
            var user = _mapper.Map<ApplicationUser>(model);
            user.UserName = model.Email;

            var creationResult = await _userManager.CreateAsync(user, model.Password);
            if (!creationResult.Succeeded)
            {
                throw new Exception($"User creation failed: {string.Join(", ", creationResult.Errors.Select(e => e.Description))}");
            }
            if(model.Roles != null && model.Roles.Any())
            {
                var roleResult = await _userManager.AddToRolesAsync(user, model.Roles);
                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user); // rollback user if role assignment fails
                    throw new Exception($"Role assignment failed: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }

            // Optionally send the token via email here or return it.
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseVM
            {
                Message = "User registered successfully",
                IsAuthenticated = true,
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles?.ToList(),
                Token = token,
            };
        }


        public Task RequestPasswordResetAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(string userId, UpdateUserRequestVM request)
        {
            throw new NotImplementedException();
        }
    }
}
