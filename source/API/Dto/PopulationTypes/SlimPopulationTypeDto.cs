using SocialWorkApi.API.Dto.Services;
using SocialWorkApi.API.Dto.Users;

namespace SocialWorkApi.API.Dto.PopulationTypes;

public class SlimPopulationTypeDto()
{
    public required int Id { get; set; }
    public required string Label { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

    //FKs
    public int? CreatedById { get; set; }

    public int? LastUpdatedById { get; set; }

}