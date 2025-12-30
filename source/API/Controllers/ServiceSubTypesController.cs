using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialWorkApi.API.Dto.ServiceSubTypes;
using SocialWorkApi.Domain.Entities.ServiceSubTypes;
using SocialWorkApi.Services.ServiceSubTypes;

namespace SocialWorkApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ServiceSubTypesController(IServiceSubTypesService _serviceTypesService) : ControllerBase
{
    private readonly IServiceSubTypesService serviceTypesService = _serviceTypesService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceSubType>>> GetAll()
    {
        return Ok(await serviceTypesService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceSubType?>> GetById(int id)
    {
        return Ok(await serviceTypesService.GetById(id));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add(CreateServiceSubTypeDto typeToCreate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        ServiceSubType type = typeToCreate.Adapt<ServiceSubType>();
        return Ok(await serviceTypesService.Add(type, parsedUserId));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateServiceSubTypeDto typeToUpdate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        ServiceSubType type = typeToUpdate.Adapt<ServiceSubType>();
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