using API.Models;
using API.DTOs;
namespace API.Interfaces;
public interface IUserService
{
    public Task<User> Get(UserDTO user);
    public Task<string?> Post(User user);
}