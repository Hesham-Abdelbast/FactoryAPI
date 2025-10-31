using DAL;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Interface.Auth
{
    public interface ITokenServices
    {
        string CreateJWTToken(IdentityUser user, List<string>? roles);
    }
}
