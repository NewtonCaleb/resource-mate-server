using SocialWorkApi.Domain.Entities.Agencies;
using SocialWorkApi.Domain.Entities.ServiceSubTypes;
using SocialWorkApi.Domain.Entities.ServiceTypes;
using SocialWorkApi.Domain.Entities.PopulationTypes;
using SocialWorkApi.Domain.Entities.Users;

namespace SocialWorkApi.Domain.Entities.Services;

public class Service
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string StreetAddress { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Zip { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public required string Cost { get; set; }
    public string? Requirements { get; set; }
    public string? Description { get; set; }
    public string? ExtraNotes { get; set; }
    public bool Deleted { get; set; }
    public DateTime LastUpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // FK
    public User? CreatedBy { get; set; }
    public int? CreatedById { get; set; }

    public User? LastUpdatedBy { get; set; }
    public int? LastUpdatedById { get; set; }

    public Agency? Agency { get; set; }
    public int? AgencyId { get; set; }

    public ServiceType? ServiceType { get; set; }
    public int? ServiceTypeId { get; set; }

    public ServiceSubType? ServiceSubType { get; set; }
    public int? ServiceSubTypeId { get; set; }

    public PopulationType? PopulationType { get; set; }
    public int? PopulationTypeId { get; set; }
}