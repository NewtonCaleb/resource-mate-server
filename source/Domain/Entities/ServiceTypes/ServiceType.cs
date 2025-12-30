using SocialWorkApi.Domain.Entities.Users;
using SocialWorkApi.Domain.Entities.ServiceSubTypes;
using SocialWorkApi.Domain.Entities.Services;

namespace SocialWorkApi.Domain.Entities.ServiceTypes;

public class ServiceType()
{
    public required int Id { get; set; }
    public required string Label { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

    //FKs
    public User? CreatedBy { get; set; }
    public int? CreatedById { get; set; }

    public User? LastUpdatedBy { get; set; }
    public int? LastUpdatedById { get; set; }

    //Children
    public ICollection<ServiceSubType>? ServiceSubTypes { get; set; }
    public ICollection<Service>? Services { get; set; }
}