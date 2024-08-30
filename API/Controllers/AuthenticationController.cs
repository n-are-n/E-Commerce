using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DTOs;
using API.Interfaces;
namespace API.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthenticationController(ILogger<AuthenticationController> logger, IUserService userService) : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger = logger;
    private readonly IUserService _userService = userService;
    [HttpPost(Name = "Register")]
    public async Task<IActionResult> SignUp([FromBody] User user)
    {
        try
        {
            _logger.LogInformation("Authentication Controller : SignUp Action");
            string? _user = await _userService.Post(user);
            return _user == null ? BadRequest(user) : Created(nameof(SignUp), user);
        }
        catch(Exception e)
        {
            _logger.LogError(e, "Authentication Controller : SignUp Action");
            return StatusCode(StatusCodes.Status406NotAcceptable, "User is not accepted");
        }
    }
    [HttpGet(Name = "Login")]
    public async Task<IActionResult> SignIn([FromQuery] UserDTO userDTO)
    {
        try
        {
            _logger.LogInformation("Authentication Controller : SignIn Action");
            User user = await _userService.Get(userDTO);
            return Accepted(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Authentication Controller : SignIn Action");
            return StatusCode(StatusCodes.Status401Unauthorized, "User is Unauthorized to access");
        }
    }
}