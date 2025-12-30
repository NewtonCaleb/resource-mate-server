using SocialWorkApi.Domain.Entities.Services;
using SocialWorkApi.Domain.Entities.ServiceTypes;
using SocialWorkApi.Domain.Entities.Users;

namespace SocialWorkApi.Domain.Entities.ServiceSubTypes;

public class ServiceSubType()
{
    public required int Id { get; set; }
    public required string Label { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

    //FKs
    public int? ServiceTypeId { get; set; }

    public User? CreatedBy { get; set; }
    public int? CreatedById { get; set; }

    public User? LastUpdatedBy { get; set; }
    public int? LastUpdatedById { get; set; }

    // Children
    public ICollection<Service>? Services { get; set; }
}