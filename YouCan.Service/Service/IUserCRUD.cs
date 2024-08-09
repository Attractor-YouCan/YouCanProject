
using YouCan.Entities;

namespace YouCan.Service.Service;

public interface IUserCRUD
{
    IEnumerable<User> GetAll();
    Task<User> GetById(int id);
    Task Insert(User entity);
    Task DeleteById(int id);
    Task Update(User entity);
}