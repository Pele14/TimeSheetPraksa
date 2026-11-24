
using Timesheet.Domain.Entities;


namespace TimeSheet.Core.Repositories;

public interface IClientRepository
{
    Task<ClientModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<ClientModel>> GetAllAsync();
    Task AddAsync(ClientModel ClientEntity);
    Task UpdateAsync(ClientModel ClientEntity);
    Task DeleteAsync(Guid id);

    Task<bool> HasProjectAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name);

}