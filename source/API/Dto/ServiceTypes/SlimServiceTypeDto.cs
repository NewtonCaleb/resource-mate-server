namespace SocialWorkApi.API.Dto.ServiceTypes;

public class SlimServiceTypeDto
{
    public required int Id { get; set; }
    public required string Label { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

    //FKs
    public int? CreatedById { get; set; }

    public int? LastUpdatedById { get; set; }
}