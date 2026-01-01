namespace SocialWorkApi.API.Dto.ServiceSubTypes;

public class CreateServiceSubTypeDto()
{
    public required string Label { get; set; }
    public required int ServiceTypeId { get; set; }
}