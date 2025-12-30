using System.Text.Json.Serialization;

namespace SocialWorkApi.API.Dto.Users;

public class UpdateUserDto
{
    public required int Id { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    [JsonIgnore]
    public DateTime LastUpdatedAt { get; set; }
    [JsonIgnore]
    public int LastUpdatedById { get; set; }
}