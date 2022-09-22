using TestTask.Shared.Entities.Abstractions.Interfaces;

namespace TestTask.WebApi.Repositories;

public abstract class RepositoryBase<T> : IRepository<T>
    where T : class, IHasKey
{
    protected readonly RepositoryContext RepositoryContext;

    protected RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }

    public virtual IEnumerable<T> GetAll() => RepositoryContext.Set<T>().OrderBy(x => x.Id).ToArray();

    public virtual T? GetById(int id) => RepositoryContext.Set<T>().FirstOrDefault(x => x.Id == id);

    public virtual void Create(T value)
    {
        T? item = GetById(value.Id);
        if (item != null)
        {
            throw new InvalidOperationException("An element with the same id already exists in the repository");
        }

        var items = GetAll();

        if (!items.Any())
        {
            value.Id = 1;
        }
        else
        {
            value.Id = GetAll().Max(x => x.Id) + 1;
        }
        
        RepositoryContext.Set<T>().Add(value);
    }

    public virtual void Update(T value)
    {
        T? item = GetById(value.Id);
        if (item == null)
        {
            throw new InvalidOperationException("It is unable to find an element with the specified id");
        }
        
        RepositoryContext.Set<T>().Remove(item);
        RepositoryContext.Set<T>().Add(value);
    }

    public virtual void Delete(T value)
    {
        T? item = GetById(value.Id);
        if (item == null)
        {
            throw new InvalidOperationException("It is unable to find an element with the specified id");
        }
        
        RepositoryContext.Set<T>().Remove(item); 
    }
}