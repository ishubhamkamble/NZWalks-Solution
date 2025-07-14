using Microsoft.AspNetCore.Identity;

namespace NzWalksAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> Roles);
    }
}
