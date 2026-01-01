namespace SocialWorkApi.API.Dto.Services;

public class UpdateServiceDto()
{
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? Cost { get; set; }
    public string? Requirements { get; set; }
    public string? Description { get; set; }
    public string? ExtraNotes { get; set; }

    // FK
    public int? AgencyId { get; set; }
    public int? ServiceTypeId { get; set; }
    public int? ServiceSubTypeId { get; set; }
    public int? PopulationTypeId { get; set; }
}