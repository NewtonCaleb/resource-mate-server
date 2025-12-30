using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialWorkApi.API.Dto.Agencies;
using SocialWorkApi.Domain.Entities.Agencies;
using SocialWorkApi.Services.Agencies;

namespace SocialWorkApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AgenciesController(IAgenciesService _agenciesService) : ControllerBase
{
    private readonly IAgenciesService agenciesService = _agenciesService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Agency>>> GetAll()
    {
        return Ok(await agenciesService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Agency?>> GetById(int id)
    {
        return Ok(await agenciesService.GetById(id));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add(CreateAgencyDto typeToCreate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        Agency type = typeToCreate.Adapt<Agency>();
        return Ok(await agenciesService.Add(type, parsedUserId));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateAgencyDto typeToUpdate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        Agency type = typeToUpdate.Adapt<Agency>();
        await agenciesService.Update(typeToUpdate, parsedUserId);
        return Ok();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        await agenciesService.Remove(id, parsedUserId);
        return Ok();
    }
}