using YouCan.Entities;
using YouCan.Repository;

namespace YouCan.Service;

public class UserCrud : IUserCrud, ICrudService<User>
{
    private IRepository<User> _repository;

    public UserCrud(IRepository<User> repository)
    {
        _repository = repository;
    }

    public IEnumerable<User> GetAll()
    {
        return _repository.GetAll();
    }

    public Task<User> GetById(int id)
    {
        return _repository.Get(id);
    }

    public async Task Insert(User entity)
    {
        await _repository.Insert(entity);
    }

    public async Task DeleteById(int id)
    {
        User entity = await _repository.Get(id);
        _repository.Remove(entity);
        await _repository.SaveChanges();
    }

    public async Task Update(User entity)
    {
        await _repository.Update(entity);
    }
}