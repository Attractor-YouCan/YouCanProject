using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouCan.Repository;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    Task<T> Get(int id);
    Task Insert(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    void Remove(T entity);
    Task SaveChanges();
}