using SocialWorkApi.Domain.Dto.PopulationTypes;
using SocialWorkApi.Domain.Entities.PopulationTypes;

namespace SocialWorkApi.Services.PopulationTypes;

public interface IPopulationTypesService
{
    public Task<PopulationType?> GetById(int id, bool enableTracking = false);
    public Task<IEnumerable<PopulationType>> GetAll();
    public Task<int> Add(PopulationType populationType, int updaterUserId);
    public Task Remove(int id, int updaterUserId);
    public Task Update(UpdatePopulationTypeDto populationType, int updaterUserId);
}