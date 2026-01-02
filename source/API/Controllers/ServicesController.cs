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
        return Ok(await servicesService.GetById(id));
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