using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.DTOs;
namespace API.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthenticationController(ILogger<AuthenticationController> logger) : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger = logger;
    [HttpPost]
    public IActionResult SignUp([FromBody] User user)
    {
        try
        {
            return Ok();
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
            return Ok();
        }
        catch(Exception e)
        {
            _logger.LogError("Authentication Controller : SignIn Action", e);
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
    }
}