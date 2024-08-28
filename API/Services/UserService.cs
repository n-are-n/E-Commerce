using API.DTOs;
using API.Models;
using API.Interfaces;
using API.Repositories;
namespace API.Services;
public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRpository;
    public UserService(ILogger<UserService> logger, UserRepository userRepository)
    {
        _logger = logger;
        _userRpository = userRepository;
    }
    public User Get(UserDTO user)
    {
        try
        {
            _logger.LogInformation("User Service : Get Method");
            // connect to repo to get user data
            return new User() { };
        }
        catch(Exception e)
        {
            _logger.LogError("User Service : Get Method", e);
            throw;
        }
    }
    public void Post(User user)
    {
        try
        {
            _logger.LogInformation("User Service : Post Method");
            // connect to repo to add user data
        }
        catch (Exception e)
        {
            _logger.LogError("User Service : Post Method", e);
            throw;
        }
    }
}