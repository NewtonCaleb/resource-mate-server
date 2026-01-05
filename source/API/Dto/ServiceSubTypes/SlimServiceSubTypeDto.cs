namespace SocialWorkApi.API.Dto.ServiceSubTypes;

public class SlimServiceSubTypeDto()
{
    public required int Id { get; set; }
    public required string Label { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }

    public int? CreatedById { get; set; }
    public int? LastUpdatedById { get; set; }
}