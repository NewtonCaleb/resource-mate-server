using Microsoft.AspNetCore.Mvc;
using SocialWorkApi.API.Dto.Users;
using SocialWorkApi.Services.Users;
using Mapster;
using SocialWorkApi.Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace SocialWorkApi.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UsersController(IUsersService usersService) : ControllerBase
{
    private readonly IUsersService _usersService = usersService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        IEnumerable<User> foundUsers = await _usersService.GetAll();

        return Ok(foundUsers.Adapt<List<UserDto>>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get(int id)
    {
        User? foundUser = await _usersService.GetById(id);

        if (foundUser == null)
        {
            return NoContent();
        }
        return Ok(foundUser.Adapt<UserDto>());
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateUserDto updatedUser)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        await _usersService.Update(updatedUser, parsedUserId);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        await _usersService.Remove(id, parsedUserId);
        return Ok();
    }
}
