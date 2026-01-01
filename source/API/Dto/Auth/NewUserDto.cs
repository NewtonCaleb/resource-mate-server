using Mapster;

namespace SocialWorkApi.API.Dto.Auth;

public class NewUserDto()
{
    public required string FirstName { get; set; }
    [AdaptIgnore]
    public required string Password { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string? Phone { get; set; }
}