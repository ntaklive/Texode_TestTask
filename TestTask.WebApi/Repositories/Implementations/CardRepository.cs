using TestTask.Shared.Entities;

namespace TestTask.WebApi.Repositories;

public class CardRepository : RepositoryBase<Card>, ICardRepository
{
    public CardRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }
}