using SocialWorkApi.API.Dto.Agencies;
using SocialWorkApi.Domain.Entities.Agencies;

namespace SocialWorkApi.Services.Agencies;

public interface IAgenciesService
{
    public Task<Agency?> GetById(int id, bool enableTracking = false);
    public Task<IEnumerable<Agency>> GetAll();
    public Task<int> Add(Agency agency, int updaterUserId);
    public Task Remove(int id, int updaterUserId);
    public Task Update(UpdateAgencyDto agency, int updaterUserId);
}