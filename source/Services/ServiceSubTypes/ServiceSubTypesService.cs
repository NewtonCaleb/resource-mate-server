using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialWorkApi.Domain.Dto.ServiceSubTypes;
using SocialWorkApi.Domain.Entities.ServiceSubTypes;
using SocialWorkApi.Services.Database;

namespace SocialWorkApi.Services.ServiceSubTypes;

public class ServiceSubTypesService(ApplicationContext dbContext) : IServiceSubTypesService
{
    async public Task<int> Add(ServiceSubType serviceType, int updaterUserId)
    {
        serviceType.CreatedAt = DateTime.UtcNow;
        serviceType.LastUpdatedAt = DateTime.UtcNow;
        serviceType.Deleted = false;
        serviceType.CreatedById = updaterUserId;
        serviceType.LastUpdatedById = updaterUserId;

        EntityEntry<ServiceSubType> createdType = await dbContext.AddAsync(serviceType);
        await dbContext.SaveChangesAsync();
        return createdType.Entity.Id;
    }

    async public Task<IEnumerable<ServiceSubType>> GetAll()
    {
        return await dbContext.Set<ServiceSubType>().Where(s => s.Deleted != true).ToListAsync();
    }

    async public Task<ServiceSubType?> GetById(int id, bool enableTracking = false)
    {
        if (enableTracking)
        {
            return await dbContext.Set<ServiceSubType>().AsTracking().Where(s => s.Deleted != true).FirstOrDefaultAsync(s => s.Id == id);
        }

        return await dbContext.Set<ServiceSubType>().Where(s => s.Deleted != true).FirstOrDefaultAsync(s => s.Id == id);
    }

    async public Task Remove(int id, int updaterUserId)
    {
        ServiceSubType? type = await GetById(id, true);
        if (type == null) return;

        type.Deleted = true;
        type.LastUpdatedById = updaterUserId;
        type.LastUpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }

    async public Task Update(UpdateServiceSubTypeDto serviceType, int updaterUserId)
    {
        ServiceSubType? originalEntity = await GetById(serviceType.Id, true);

        if (originalEntity == null)
        {
            return;
        }

        originalEntity.LastUpdatedAt = DateTime.UtcNow;
        originalEntity.LastUpdatedById = updaterUserId;

        dbContext.Set<ServiceSubType>().Update(EntityMerger.Merge(serviceType, originalEntity));
        await dbContext.SaveChangesAsync();
    }
}