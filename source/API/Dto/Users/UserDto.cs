namespace SocialWorkApi.API.Dto.Users
{
    public class UserDto
    {
        public required int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public UserDto? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public UserDto? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool? Deleted { get; set; }
    }
}

