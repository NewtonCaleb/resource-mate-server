namespace SocialWorkApi.API.Dto.Services;

public class CreateServiceDto()
{
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

    // FK
    public required int AgencyId { get; set; }
    public required int ServiceTypeId { get; set; }
    public int? ServiceSubTypeId { get; set; }
    public required int PopulationTypeId { get; set; }
}