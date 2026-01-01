using SocialWorkApi.Domain.Entities.Services;
using SocialWorkApi.Domain.Entities.Users;

namespace SocialWorkApi.Domain.Entities.Agencies;

public class Agency
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Zip { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public bool Deleted { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }


    // FK
    public User? LastUpdatedBy { get; set; }
    public int? LastUpdatedById { get; set; }
    public User? CreatedBy { get; set; }
    public int? CreatedById { get; set; }

    // Children
    public ICollection<Service>? Services { get; set; }
}