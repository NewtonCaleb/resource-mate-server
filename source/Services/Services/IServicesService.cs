using SocialWorkApi.API.Dto.Services;
using SocialWorkApi.Domain.Entities.Services;

namespace SocialWorkApi.Services.Services;

public interface IServicesService
{
    public Task<Service?> GetById(int id, bool enableTracking = false);
    public Task<IEnumerable<Service>> GetAll();
    public Task<int> Add(Service service, int updaterUserId);
    public Task Remove(int id, int updaterUserId);
    public Task Update(UpdateServiceDto service, int updaterUserId);
}