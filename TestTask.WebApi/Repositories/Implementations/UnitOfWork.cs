namespace TestTask.WebApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RepositoryContext _context;

    public UnitOfWork(RepositoryContext context)
    {
        _context = context;
    }
    
    private ICardRepository? _cards;
    public ICardRepository Cards => _cards ??= new CardRepository(_context);
    
    public void Save()
    {
        _context.SaveChanges();
    }
}