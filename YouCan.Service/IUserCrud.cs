using YouCan.Entities;

namespace YouCan.Service;

public interface IUserCrud
{
    IEnumerable<User> GetAll();
    Task<User> GetById(int id);
    Task Insert(User entity);
    Task DeleteById(int id);
    Task Update(User entity);
}