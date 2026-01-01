using SocialWorkApi.API.Dto.Users;

namespace SocialWorkApi.API.Dto.Agencies;

public class AgencyDto()
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public string? Zip { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Description { get; set; }
    public bool Deleted { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }


    // FK
    public UserDto? LastUpdatedBy { get; set; }
    public UserDto? CreatedBy { get; set; }

    // Children
    // public ICollection<Service>? Services { get; set; }
}