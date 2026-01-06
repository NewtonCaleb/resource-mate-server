namespace SocialWorkApi.API.Dto.Services;

public class SlimServiceDto
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
    public int CreatedById { get; set; }

    public int LastUpdatedById { get; set; }
}