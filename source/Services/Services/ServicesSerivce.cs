using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialWorkApi.Domain.Dto.Services;
using SocialWorkApi.Domain.Entities.Services;
using SocialWorkApi.Services.Database;

namespace SocialWorkApi.Services.Services;

public class ServicesService(ApplicationContext dbContext) : IServicesService
{
    async public Task<int> Add(Service service, int updaterUserId)
    {
        service.CreatedAt = DateTime.UtcNow;
        service.LastUpdatedAt = DateTime.UtcNow;
        service.Deleted = false;
        service.CreatedById = updaterUserId;
        service.LastUpdatedById = updaterUserId;

        EntityEntry<Service> createdType = await dbContext.AddAsync(service);
        await dbContext.SaveChangesAsync();
        return createdType.Entity.Id;
    }

    async public Task<IEnumerable<Service>> GetAll()
    {
        return await dbContext.Set<Service>().Where(s => s.Deleted != true).ToListAsync();
    }

    async public Task<Service?> GetById(int id, bool enableTracking = false)
    {
        if (enableTracking)
        {
            return await dbContext.Set<Service>().AsTracking().Where(s => s.Deleted != true).FirstOrDefaultAsync(s => s.Id == id);
        }

        return await dbContext.Set<Service>().Where(s => s.Deleted != true).FirstOrDefaultAsync(s => s.Id == id);
    }

    async public Task Remove(int id, int updaterUserId)
    {
        Service? type = await GetById(id, true);
        if (type == null) return;

        type.Deleted = true;
        type.LastUpdatedById = updaterUserId;
        type.LastUpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }

    async public Task Update(UpdateServiceDto service, int updaterUserId)
    {
        Service? originalEntity = await GetById(service.Id, true);

        if (originalEntity == null)
        {
            return;
        }

        originalEntity.LastUpdatedAt = DateTime.UtcNow;
        originalEntity.LastUpdatedById = updaterUserId;

        dbContext.Set<Service>().Update(EntityMerger.Merge(service, originalEntity));
        await dbContext.SaveChangesAsync();
    }
}