using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DTOs;
using API.Interfaces;
using API.Services;
namespace API.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IUserService _userService;
    public AuthenticationController(ILogger<AuthenticationController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    [HttpPost]
    public IActionResult SignUp([FromBody] User user)
    {
        try
        {
            _logger.LogInformation("Authentication Controller : SignUp Action");
            _userService.Post(user);
            return Created();
        }
        catch(Exception e)
        {
            _logger.LogError("Authentication Controller : SignUp Action", e);
            return StatusCode(StatusCodes.Status406NotAcceptable);
        }
    }
    [HttpGet]
    public IActionResult SignIn(UserDTO user)
    {
        try
        {
            _logger.LogInformation("Authentication Controller : SignIn Action");
            _userService.Get(user);
            return Accepted();
        }
        catch(Exception e)
        {
            _logger.LogError("Authentication Controller : SignIn Action", e);
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
    }
}