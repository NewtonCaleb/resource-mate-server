using SocialWorkApi.Domain.Dto.Users;
using SocialWorkApi.Domain.Entities.Users;
namespace SocialWorkApi.Services.Users;

public interface IUsersService
{
    public Task<User?> GetById(int id, bool enableTracking = false);
    public Task<IEnumerable<User>> GetAll();
    public Task Remove(int id, int updaterUserId);
    public Task Update(UpdateUserDto userToUpdate, int updaterUserId);
}