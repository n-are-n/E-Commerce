using API.Interfaces;
using API.Models;
namespace API.Services;
public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
    }
    public User Get(int id)
    {
        try
        {
            return new User { };
        }
        catch(Exception e)
        {
            _logger.LogError("User Service : Get Method", e);
            throw;
        }
    }
    public void Post(User user)
    {}
}