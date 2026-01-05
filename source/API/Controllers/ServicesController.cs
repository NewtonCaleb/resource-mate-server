using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialWorkApi.API.Dto.Services;
using SocialWorkApi.Domain.Entities.Services;
using SocialWorkApi.Services.Services;

namespace SocialWorkApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ServicesController(IServicesService _servicesService) : ControllerBase
{
    private readonly IServicesService servicesService = _servicesService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAll()
    {
        return Ok(await servicesService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto?>> GetById(int id)
    {
        Service? service = await servicesService.GetById(id);
        if (service == null)
        {
            return NoContent();
        }

        //     var config = new TypeAdapterConfig();
        //     config.NewConfig<Service, ServiceDto>()
        // .Ignore(dest => dest.ServiceType.Services)
        // .Ignore(dest => dest.ServiceType.ServiceSubTypes)
        // .Ignore(dest => dest.ServiceSubType.Services)
        // .Ignore(dest => dest.PopulationType.Services);

        try
        {
            service.Adapt<ServiceDto>();
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return Ok(service.Adapt<ServiceDto>());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add(CreateServiceDto serviceToCreate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        Service service = serviceToCreate.Adapt<Service>();
        return Ok(await servicesService.Add(service, parsedUserId));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateServiceDto serviceToUpdate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        await servicesService.Update(serviceToUpdate, parsedUserId);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        await servicesService.Remove(id, parsedUserId);
        return Ok();
    }
}