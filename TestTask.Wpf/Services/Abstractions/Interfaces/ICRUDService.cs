using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestTask.Wpf.Services.Abstractions;

public interface ICRUDService<T, TDtoForCreation, TDtoForUpdate>
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task<T> CreateAsync(TDtoForCreation value);
    Task UpdateAsync(int id, TDtoForUpdate value);
    Task DeleteAsync(int id);
}