namespace SocialWorkApi.API.Dto.Agencies;

public class CreateAgencyDto()
{
    public required string Name { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string Zip { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
}