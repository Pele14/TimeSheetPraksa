
using Timesheet.Domain.Entities;


namespace TimeSheet.Core.Repositories;

public interface IClientRepository
{
    Task<ClientEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<ClientEntity>> GetAllAsync();
    Task AddAsync(ClientEntity ClientEntity);
    Task UpdateAsync(ClientEntity ClientEntity);
    Task DeleteAsync(Guid id);

    Task<bool> HasProjectAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);

}