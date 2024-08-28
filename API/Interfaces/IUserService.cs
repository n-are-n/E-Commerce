using API.DTOs;
using API.Models;
namespace API.Interfaces;
public interface IUserService
{
    public User Get(UserDTO user);
    public void Post(User user);
}