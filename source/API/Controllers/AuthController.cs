using Mapster;
using Microsoft.AspNetCore.Mvc;
using SocialWorkApi.API.Dto.Auth;
using SocialWorkApi.Domain.Entities.Users;
using SocialWorkApi.API.Dto.Users;
using SocialWorkApi.Services.Auth;
using SocialWorkApi.Services.Users;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SocialWorkApi.API.Controllers;

[ApiController]
public class AuthController(IAuthService _authService, IValidator<LoginDto> _validator) : ControllerBase
{
    private readonly IAuthService authService = _authService;
    private readonly IValidator<LoginDto> validator = _validator;

    [HttpPost("Users/NewUser")]
    [Authorize]
    public async Task<ActionResult<int>> CreateNewUser(NewUserDto newUser)
    {
        string? userId = User.FindFirstValue("Id");
        if (userId == null) return Unauthorized();
        bool parseSuccessful = int.TryParse(userId, out int parsedUserId);
        if (!parseSuccessful) throw new Exception("Unable to parse UserId");

        int? insertedId = await authService.CreateUser(newUser, parsedUserId);
        if (insertedId == null)
        {
            return BadRequest("Invalid data");
        }

        return Ok(insertedId);
    }

    [HttpPost("Users/Login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto login)
    {
        Console.WriteLine(login.Password);
        Console.WriteLine();
        Console.WriteLine(login.Email);
        validator.ValidateAndThrow(login);

        string? token = await authService.Login(login.Email!, login.Password!);
        if (token == null)
        {
            return BadRequest("Invalid login");
        }

        return Ok(new { token });
    }
}