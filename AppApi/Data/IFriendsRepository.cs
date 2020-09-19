using System.Collections.Generic;
using System.Threading.Tasks;
using AppApi.Models;

namespace AppApi.Data
{
  public interface IFriendsRepository
  {
    void Add<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task<bool> SaveAll();
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUser(int id);
  }
}