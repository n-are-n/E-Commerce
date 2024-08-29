using Microsoft.EntityFrameworkCore;
using API.Models;
using API.DTOs;
using API.Interfaces;
using API.Data;
namespace API.Repositories;
public class UserRepository(ILogger<UserRepository> logger, EcommerceContext context) : IUserRepository
{
    private readonly ILogger<UserRepository> _logger = logger;
    private readonly EcommerceContext _context = context;
    public User GetUser(UserDTO userDTO)
    {
        try
        {
            _logger.LogInformation("User Repository : Get User Function");
            User user = _context.Users.FirstAsync(x => x.Number == userDTO.Number && x.Password == userDTO.Password).Result;
            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "User Repository : Get User Function");
            throw;
        }
    }
    public async Task<bool> AddUser(User user)
    {
        try
        {
            _logger.LogInformation("User Repository : Add User Function");
            await _context.Users.AddAsync(user);
            int affectedRows = await _context.SaveChangesAsync();
            return affectedRows == 1;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "User Repository : Add User Function");
            throw;
        }
    }
}