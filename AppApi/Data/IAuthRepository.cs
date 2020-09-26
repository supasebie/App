using System.Threading.Tasks;
using AppApi.Models;

namespace AppApi.Data
{
  public interface IAuthRepository
  {
    Task<User> Register(User user, string password);
    Task<User> Login(string username, string password);
    Task<bool> UserExists(string username);
  }
}