using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialWorkApi.API.Dto.PopulationTypes;
using SocialWorkApi.Domain.Entities.PopulationTypes;
using SocialWorkApi.Services.PopulationTypes;

namespace SocialWorkApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PopulationTypesController(IPopulationTypesService _populationTypesService) : ControllerBase
{
    private readonly IPopulationTypesService populationTypesService = _populationTypesService;

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<PopulationType>>> GetAll()
    {
        return Ok(await populationTypesService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PopulationType?>> GetById(int id)
    {
        return Ok(await populationTypesService.GetById(id));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add(CreatePopulationTypeDto typeToCreate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        PopulationType type = typeToCreate.Adapt<PopulationType>();
        return Ok(await populationTypesService.Add(type, parsedUserId));
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdatePopulationTypeDto typeToUpdate)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");
        
        await populationTypesService.Update(typeToUpdate, parsedUserId);
        return Ok();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        await populationTypesService.Remove(id, parsedUserId);
        return Ok();
    }
}