using API.Models;
using API.DTOs;
using API.Interfaces;
using StackExchange.Redis;
using Newtonsoft.Json;
namespace API.Services;
public class UserService(ILogger<UserService> logger, IConnectionMultiplexer redis, IUserRepository userRepository) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly IConnectionMultiplexer _redis = redis;
    private readonly IUserRepository _userRpository = userRepository;
    public async Task<User> Get(UserDTO userDTO)
    {
        try
        {
            User user;
            _logger.LogInformation("User Service : Get Method");
            var db = _redis.GetDatabase();
            RedisValue session = await db.StringGetAsync($"user:{userDTO.Number}");
            if(!session.IsNullOrEmpty) user = JsonConvert.DeserializeObject<User>(session!)!;
            else
            {
                user = _userRpository.GetUser(userDTO);
                string userString = JsonConvert.SerializeObject(user);
                await db.StringSetAsync($"user:{user.Number}", userString, TimeSpan.FromMinutes(5));
            }
            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "User Service : Get Method");
            throw;
        }
    }
    public async Task<string?> Post(User user)
    {
        try
        {
            _logger.LogInformation("User Service : Post Method");
            bool success = await _userRpository.AddUser(user);
            return success == true ? "User Added Successfully:-)" : null;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "User Service : Post Method");
            throw;
        }
    }
}