using YouCan.Entities;
using YouCan.Repository;

namespace YouCan.Service;

public class CrudService<T> : ICrudService<T> where T : EntityBase
{
    private IRepository<T> _repository;

    public CrudService(IRepository<T> repository)
    {
        _repository = repository;
    }

    public IEnumerable<T> GetAll()
    {
        return _repository.GetAll();
    }

    public Task<T> GetById(int id)
    {
        return _repository.Get(id);
    }

    public async Task Insert(T entity)
    {
        await _repository.Insert(entity);
    }

    public async Task DeleteById(int id)
    {
        T entity = await _repository.Get(id);
        _repository.Remove(entity);
        await _repository.SaveChanges();
    }

    public async Task Update(T entity)
    {
        await _repository.Update(entity);
    }
}