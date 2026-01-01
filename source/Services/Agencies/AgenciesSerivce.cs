using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialWorkApi.API.Dto.Agencies;
using SocialWorkApi.Domain.Entities.Agencies;
using SocialWorkApi.Services.Database;

namespace SocialWorkApi.Services.Agencies;

public class AgenciesService(ApplicationContext dbContext) : IAgenciesService
{
    async public Task<int> Add(Agency agency, int updaterUserId)
    {
        agency.CreatedAt = DateTime.UtcNow;
        agency.LastUpdatedAt = DateTime.UtcNow;
        agency.Deleted = false;
        agency.CreatedById = updaterUserId;
        agency.LastUpdatedById = updaterUserId;

        EntityEntry<Agency> createdType = await dbContext.AddAsync(agency);
        await dbContext.SaveChangesAsync();
        return createdType.Entity.Id;
    }

    async public Task<IEnumerable<Agency>> GetAll()
    {
        return await dbContext.Set<Agency>().Where(s => s.Deleted != true).ToListAsync();
    }

    async public Task<Agency?> GetById(int id, bool enableTracking = false)
    {
        if (enableTracking)
        {
            return await dbContext.Set<Agency>().AsTracking().Where(s => s.Deleted != true).FirstOrDefaultAsync(s => s.Id == id);
        }

        return await dbContext.Set<Agency>().Where(s => s.Deleted != true).Include(a => a.CreatedBy).Include(a => a.LastUpdatedBy).FirstOrDefaultAsync(s => s.Id == id);
    }

    async public Task Remove(int id, int updaterUserId)
    {
        Agency? type = await GetById(id, true);
        if (type == null) return;

        type.Deleted = true;
        type.LastUpdatedById = updaterUserId;
        type.LastUpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync();
    }

    async public Task Update(UpdateAgencyDto agency, int updaterUserId)
    {
        Agency? originalEntity = await GetById(agency.Id, true);

        if (originalEntity == null)
        {
            return;
        }

        originalEntity.LastUpdatedAt = DateTime.UtcNow;
        originalEntity.LastUpdatedById = updaterUserId;

        dbContext.Set<Agency>().Update(EntityMerger.Merge(agency, originalEntity));
        await dbContext.SaveChangesAsync();
    }
}