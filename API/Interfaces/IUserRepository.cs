using API.Models;
using API.DTOs;
namespace API.Interfaces;
public interface IUserRepository
{
    public User GetUser(UserDTO user);
    public Task<bool> AddUser(User user);
}