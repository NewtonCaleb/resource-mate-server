using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialWorkApi.API.Dto.ServiceTypes;
using SocialWorkApi.Domain.Entities.ServiceTypes;
using SocialWorkApi.Services.Database;

namespace SocialWorkApi.Services.ServiceTypes;

public class ServiceTypesService(ApplicationContext dbContext) : IServiceTypesService
{
    async public Task<int> Add(ServiceType serviceType, int updaterUserId)
    {
        serviceType.CreatedAt = DateTime.UtcNow;
        serviceType.LastUpdatedAt = DateTime.UtcNow;
        serviceType.Deleted = false;
        serviceType.CreatedById = updaterUserId;
        serviceType.LastUpdatedById = updaterUserId;

        EntityEntry<ServiceType> createdType = await dbContext.AddAsync(serviceType);
        await dbContext.SaveChangesAsync();
        return createdType.Entity.Id;
    }

    async public Task<IEnumerable<ServiceType>> GetAll()
    {
        return await dbContext.Set<ServiceType>().Where(s => s.Deleted != true).Include(s => s.ServiceSubTypes!.Where(sub => sub.Deleted != true)).ToListAsync();
    }

    async public Task<ServiceType?> GetById(int id, bool enableTracking = false)
    {
        if (enableTracking)
        {
            return await dbContext.Set<ServiceType>().AsTracking().Where(s => s.Deleted != true).Include(s => s.ServiceSubTypes!.Where(sub => sub.Deleted != true)).FirstOrDefaultAsync(s => s.Id == id);
        }

        return await dbContext.Set<ServiceType>().Where(s => s.Deleted != true).FirstOrDefaultAsync(s => s.Id == id);
    }

    async public Task Remove(int id, int updaterUserId)
    {
        ServiceType? type = await GetById(id, true);
        if (type == null) return;

        type.Deleted = true;
        type.LastUpdatedById = updaterUserId;
        type.LastUpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }

    async public Task Update(UpdateServiceTypeDto serviceType, int updaterUserId)
    {
        ServiceType? originalEntity = await GetById(serviceType.Id, true);

        if (originalEntity == null)
        {
            return;
        }

        originalEntity.LastUpdatedAt = DateTime.UtcNow;
        originalEntity.LastUpdatedById = updaterUserId;

        dbContext.Set<ServiceType>().Update(EntityMerger.Merge(serviceType, originalEntity));
        await dbContext.SaveChangesAsync();
    }
}