
using Microsoft.AspNetCore.Mvc;
using TimeSheet.Application.Auth;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.AuthRequests;
using TimeSheet.Application.Responses.AuthResponses;

namespace TimeSheet.Auth.Api.Controllers;


[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService AuthService;

    public AuthController(IAuthService authService)
    {
        AuthService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {

        return Ok(await AuthService.LoginAsync(request));


    }
}