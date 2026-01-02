using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialWorkApi.API.Dto.Services;
using SocialWorkApi.Domain.Entities.Agencies;
using SocialWorkApi.Domain.Entities.Services;
using SocialWorkApi.Services.Agencies;
using SocialWorkApi.Services.Database;
using SocialWorkApi.Services.PopulationTypes;
using SocialWorkApi.Services.ServiceSubTypes;
using SocialWorkApi.Services.ServiceTypes;

namespace SocialWorkApi.Services.Services;

public class ServicesService(ApplicationContext dbContext, IAgenciesService _agenciesService, IServiceTypesService _serviceTypesService, IServiceSubTypesService _serviceSubTypesService, IPopulationTypesService _populationTypesService) : IServicesService
{
    async public Task<int> Add(Service service, int updaterUserId)
    {
        if (!service.AgencyId.HasValue || !service.ServiceTypeId.HasValue || !service.PopulationTypeId.HasValue) throw new Exception("Requires agency, service type, and population type");

        service.CreatedAt = DateTime.UtcNow;
        service.LastUpdatedAt = DateTime.UtcNow;
        service.Deleted = false;
        service.CreatedById = updaterUserId;
        service.LastUpdatedById = updaterUserId;

        // This is used to validate that the items are real
        try
        {
            _ = await _agenciesService.GetById(service.AgencyId.Value) ?? throw new Exception("Unable to find agency");
            _ = await _serviceTypesService.GetById(service.ServiceTypeId.Value) ?? throw new Exception("Unable to find service type");
            _ = await _populationTypesService.GetById(service.PopulationTypeId.Value) ?? throw new Exception("Unable to find population type");

            if (service.ServiceSubTypeId.HasValue)
            {
                _ = await _serviceSubTypesService.GetById(service.ServiceSubTypeId.Value) ?? throw new Exception("Unable to find service sub type");
            }
        }
        catch (System.Exception)
        {
            throw;
        }


        EntityEntry<Service> createdService = await dbContext.AddAsync(service);
        await dbContext.SaveChangesAsync();
        return createdService.Entity.Id;
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

        return await dbContext.Set<Service>()
        .Include(s => s.CreatedBy)
        .Include(s => s.LastUpdatedBy)
        .Include(s => s.Agency)
        .Include(s => s.ServiceType)
        .Include(s => s.ServiceSubType)
        .Include(s => s.PopulationType)
        .Where(s => s.Deleted != true)
        .FirstOrDefaultAsync(s => s.Id == id);
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