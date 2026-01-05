using SocialWorkApi.API.Dto.Services;
using SocialWorkApi.API.Dto.ServiceSubTypes;
using SocialWorkApi.API.Dto.Users;

namespace SocialWorkApi.API.Dto.ServiceTypes;

public class ServiceTypeDto
{
    public required int Id { get; set; }
    public required string Label { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

    //FKs
    public UserDto? CreatedBy { get; set; }
    public int? CreatedById { get; set; }

    public UserDto? LastUpdatedBy { get; set; }
    public int? LastUpdatedById { get; set; }

    //Children
    public ICollection<ServiceSubTypeDto>? ServiceSubTypes { get; set; }
    public ICollection<ServiceDto>? Services { get; set; }
}