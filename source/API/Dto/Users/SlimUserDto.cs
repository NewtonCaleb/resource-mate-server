namespace SocialWorkApi.API.Dto.Users
{
    public class SlimUserDto
    {
        public required int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? LastUpdatedById { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool? Deleted { get; set; }
    }
}

