using TestTask.Shared.Entities.Abstractions.Interfaces;

namespace TestTask.WebApi.Repositories;

public interface IRepository<T> where T: class, IHasKey
{
    IEnumerable<T> GetAll();
    T? GetById(int id);
    void Create(T value);
    void Update(T value);
    void Delete(T value);
}