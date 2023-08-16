using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

public interface IUserAuthenticationRepository
{
    Task<string> CreateTokenAsync(IdentityUser user);
}