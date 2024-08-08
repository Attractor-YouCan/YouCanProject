namespace YouCan.Service.Service;

public interface ICRUDService<T> where T : class
{
    IEnumerable<T> GetAll();
    Task<T> GetById(int id);
    Task Insert(T entity);
    Task DeleteById(int id);
    Task Update(T entity);
}