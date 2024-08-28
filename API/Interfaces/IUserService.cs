using API.Models;
namespace API.Interfaces;
public interface IUserService
{
    public User Get(int id);
    public void Post(User user);
}