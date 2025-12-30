using SocialWorkApi.Domain.Dto.ServiceSubTypes;
using SocialWorkApi.Domain.Dto.ServiceTypes;
using SocialWorkApi.Domain.Entities.ServiceSubTypes;
using SocialWorkApi.Domain.Entities.ServiceTypes;

namespace SocialWorkApi.Services.ServiceSubTypes;

public interface IServiceSubTypesService
{
    public Task<ServiceSubType?> GetById(int id, bool enableTracking = false);
    public Task<IEnumerable<ServiceSubType>> GetAll();
    public Task<int> Add(ServiceSubType serviceType, int updaterUserId);
    public Task Remove(int id, int updaterUserId);
    public Task Update(UpdateServiceSubTypeDto serviceType, int updaterUserId);
}