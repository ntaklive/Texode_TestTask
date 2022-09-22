namespace TestTask.WebApi.Repositories
{
    public interface IUnitOfWork 
    {
        ICardRepository Cards { get; } 
        
        void Save(); 
    }
}
