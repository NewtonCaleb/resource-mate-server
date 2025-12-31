using SocialWorkApi.API.Dto.Auth;

namespace SocialWorkApi.Services.Auth;

public interface IAuthService
{
    public Task<int> CreateUser(NewUserDto user, int updaterUserId);
    public Task<string?> Login(string email, string pwd);
}