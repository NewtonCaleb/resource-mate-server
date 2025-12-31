using SocialWorkApi.API.Dto.ServiceTypes;
using SocialWorkApi.Domain.Entities.ServiceTypes;

namespace SocialWorkApi.Services.ServiceTypes;

public interface IServiceTypesService
{
    public Task<ServiceType?> GetById(int id, bool enableTracking = false);
    public Task<IEnumerable<ServiceType>> GetAll();
    public Task<int> Add(ServiceType serviceType, int updaterUserId);
    public Task Remove(int id, int updaterUserId);
    public Task Update(UpdateServiceTypeDto serviceType, int updaterUserId);
}