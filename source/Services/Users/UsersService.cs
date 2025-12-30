using SocialWorkApi.Domain.Entities.Users;
using SocialWorkApi.Services.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialWorkApi.Domain.Dto.Users;

namespace SocialWorkApi.Services.Users;

public class UsersService(ApplicationContext _dbContext) : IUsersService
{
    private readonly ApplicationContext db = _dbContext;

    public async Task<IEnumerable<User>> GetAll()
    {
        return await db.Set<User>().Include(u => u.CreatedBy).Include(u => u.LastUpdatedBy).ToListAsync();
    }

    public async Task<User?> GetById(int id, bool enableTracking = false)
    {
        if (!enableTracking)
        {
            return await db.Set<User>().FirstOrDefaultAsync((u) => u.Id == id);
        }
        else
        {
            return await db.Set<User>().AsTracking().FirstOrDefaultAsync((u) => u.Id == id);
        }
    }

    public async Task Remove(int id, int updaterUserId)
    {
        User? userToRemove = await GetById(id, true);
        if (userToRemove == null)
        {
            return;
        }        

        db.Remove(userToRemove);
        await db.SaveChangesAsync();
    }

    public async Task Update(UpdateUserDto userToUpdate, int updatedUserId)
    {
        User? originalEntity = await GetById(userToUpdate.Id, true) ?? throw new Exception("Entity not found with this Id");
        userToUpdate.LastUpdatedAt = DateTime.UtcNow;
        userToUpdate.LastUpdatedById = updatedUserId;

        db.Update<User>(EntityMerger.Merge(userToUpdate,originalEntity));
        await db.SaveChangesAsync();
    }
}