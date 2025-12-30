using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialWorkApi.API.Dto.ServiceTypes;
using SocialWorkApi.Domain.Entities.ServiceTypes;
using SocialWorkApi.Services.ServiceTypes;

namespace SocialWorkApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ServiceTypesController(IServiceTypesService _serviceTypesService) : ControllerBase
{
    private readonly IServiceTypesService serviceTypesService = _serviceTypesService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceType>>> GetAll()
    {
        return Ok(await serviceTypesService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceType?>> GetById(int id)
    {
        return Ok(await serviceTypesService.GetById(id));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add(CreateServiceTypeDto typeToCreate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        ServiceType type = typeToCreate.Adapt<ServiceType>();
        return Ok(await serviceTypesService.Add(type, parsedUserId));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateServiceTypeDto typeToUpdate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        ServiceType type = typeToUpdate.Adapt<ServiceType>();
        await serviceTypesService.Update(typeToUpdate, parsedUserId);
        return Ok();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        await serviceTypesService.Remove(id, parsedUserId);
        return Ok();
    }
}