namespace SocialWorkApi.Services.Auth;

using System.Text;

using SocialWorkApi.Services.Database;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using SocialWorkApi.Domain.Entities.Users;
using SocialWorkApi.Domain.Dto.Auth;
using Mapster;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

public class AuthSerivce(ApplicationContext _dbContext, IOptions<AuthOptions> _authOptions) : IAuthService
{
    private readonly AuthOptions options = _authOptions.Value;
    private readonly ApplicationContext dbContext = _dbContext;

    public async Task<int> CreateUser(NewUserDto userToCreate, int updaterUserId)
    {
        User u = userToCreate.Adapt<User>();
        u.Password = Encoding.UTF8.GetBytes(BCrypt.HashPassword(userToCreate.Password));
        u.CreatedAt = DateTime.UtcNow;
        u.LastUpdatedAt = DateTime.UtcNow;
        u.LastUpdatedById = updaterUserId;
        u.CreatedById = updaterUserId;
        
        EntityEntry<User> addedUser = await dbContext.AddAsync(u);
        await dbContext.SaveChangesAsync();

        return addedUser.Entity.Id;
    }

    public async Task<string?> Login(string email, string pwd)
    {
        User? foundUser = await dbContext.Set<User>().AsNoTracking().FirstOrDefaultAsync((u) => u.Email == email);
        if (foundUser == default || foundUser.Password == null) return null;

        if (!BCrypt.Verify(pwd, Encoding.UTF8.GetString(foundUser.Password)))
        {
            return null;
        }

        return GenerateUserToken(Convert.ToInt32(foundUser.Id));
    }

    private string GenerateUserToken(int userId)
    {
        SigningCredentials credentials = new(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.JwtSecret)), SecurityAlgorithms.HmacSha512);
        JwtSecurityToken jwtOptions = new(
            issuer: "https://localhost:7258",
            audience: "https://localhost:7258",
            claims: [new Claim("Id", userId.ToString())],
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

        string token = new JwtSecurityTokenHandler().WriteToken(jwtOptions);
        return token;
    }

    private string ValidateJwtToken()
    {
        throw new NotImplementedException();
    }
}