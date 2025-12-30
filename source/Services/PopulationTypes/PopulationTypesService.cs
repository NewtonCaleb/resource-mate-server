using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialWorkApi.Domain.Dto.PopulationTypes;
using SocialWorkApi.Domain.Entities.PopulationTypes;
using SocialWorkApi.Services.Database;

namespace SocialWorkApi.Services.PopulationTypes;

public class PopulationTypesService(ApplicationContext dbContext) : IPopulationTypesService
{
    async public Task<int> Add(PopulationType populationType, int updaterUserId)
    {
        populationType.CreatedAt = DateTime.UtcNow;
        populationType.LastUpdatedAt = DateTime.UtcNow;
        populationType.Deleted = false;
        populationType.CreatedById = updaterUserId;
        populationType.LastUpdatedById = updaterUserId;

        EntityEntry<PopulationType> createdType = await dbContext.AddAsync(populationType);
        await dbContext.SaveChangesAsync();
        return createdType.Entity.Id;
    }

    async public Task<IEnumerable<PopulationType>> GetAll()
    {
        return await dbContext.Set<PopulationType>().Where(p => p.Deleted != true).ToListAsync();
    }

    async public Task<PopulationType?> GetById(int id, bool enableTracking = false)
    {
        if (enableTracking)
        {
            return await dbContext.Set<PopulationType>().AsTracking().Where(p => p.Deleted != true).FirstOrDefaultAsync(p => p.Id == id);
        }

        return await dbContext.Set<PopulationType>().Where(p => p.Deleted != true).FirstOrDefaultAsync(p => p.Id == id);
    }

    async public Task Remove(int id, int updaterUserId)
    {
        PopulationType? type = await GetById(id, true);
        if (type == null) return;

        type.Deleted = true;
        type.LastUpdatedAt = DateTime.UtcNow;
        type.LastUpdatedById = updaterUserId;
        await dbContext.SaveChangesAsync();
    }

    async public Task Update(UpdatePopulationTypeDto populationType, int updaterUserId)
    {
        PopulationType? originalEntity = await GetById(populationType.Id, true);

        if (originalEntity == null)
        {
            return;
        }

        originalEntity.LastUpdatedAt = DateTime.UtcNow;
        originalEntity.LastUpdatedById = updaterUserId;

        dbContext.Set<PopulationType>().Update(EntityMerger.Merge(populationType, originalEntity));
        await dbContext.SaveChangesAsync();
    }
}