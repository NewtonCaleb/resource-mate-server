using System.Text.Json.Serialization;

namespace SocialWorkApi.Domain.Entities.Users
{
    public class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public bool Deleted { get; set; }

        //FKS
        // Creator
        public User? CreatedBy { get; set; }
        public int? CreatedById { get; set; }

        // Updater
        public User? LastUpdatedBy { get; set; }
        public int? LastUpdatedById { get; set; }

        // TODO: Look into consolidating this and getting it out of the User Model
        [JsonIgnore]
        public byte[]? Password { get; set; }
    }
}