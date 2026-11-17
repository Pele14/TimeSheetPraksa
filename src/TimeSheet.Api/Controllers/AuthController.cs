using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.Auth;
using TimeSheet.Application.Auth.DTOs;
using TimeSheet.Application.Common.Exceptions;

namespace TimeSheet.Api.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController: ControllerBase
{
    private readonly IAuthService AuthService;

    public AuthController(IAuthService authService)
    {
        AuthService=authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>>Login(LoginDTO dto)
    {
        try
        {
            return Ok(await AuthService.LoginAsync(dto));
        }
        catch(UnauthorizedException ex)
        {
            return Unauthorized(new{message=ex.Message});
        }

    }
}