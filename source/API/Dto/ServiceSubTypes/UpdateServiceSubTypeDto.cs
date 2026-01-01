namespace SocialWorkApi.API.Dto.ServiceSubTypes;

public class UpdateServiceSubTypeDto()
{
    public required int Id { get; set; }
    public string? Label { get; set; }
    public int? ServiceTypeId { get; set; }
}