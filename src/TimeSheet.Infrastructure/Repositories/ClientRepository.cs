using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Core.Repositories;
using Timesheet.Domain.Entities;
using TimeSheet.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
using TimeSheet.Infrastructure.Configurations;
namespace TimeSheet.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClientModel?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Clients.FindAsync(id);
        return entity?.MapToDomain();
    }

    public async Task<IEnumerable<ClientModel>> GetAllAsync()
    {
        var entities = await _context.Clients.OrderBy(c=>c.Name).ToListAsync();
        return entities.Select(e => e.MapToDomain()).ToList();
    }

    public async Task AddAsync(ClientModel client)
    {
        await _context.Clients.AddAsync(client.MapToEntity());
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ClientModel client)
    {
        var entity = await _context.Clients.FindAsync(client.Id)
          ?? throw new KeyNotFoundException($"Client {client.Id} doesnt exist.");

        client.UpdateEntity(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            var proects=await _context.Projects
                .IgnoreQueryFilters([ProjectFilters.ActiveOnly])
                .Where(p=>p.ClientId==id)
                .ToListAsync();

            var timeEntries=await _context.TimeEntries
                .IgnoreQueryFilters([ProjectFilters.ActiveOnly])
                .Where(te=>te.Project.ClientId==id)
                .ToListAsync();    

            _context.TimeEntries.RemoveRange(timeEntries);
            _context.Projects.RemoveRange(proects);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
        else
            throw new KeyNotFoundException("Id doesnt exist");
    }
    public async Task<bool> ExistsByNameAsync(string name)
    {
        var normalized = name.Trim().ToLower();
        return await _context.Clients.AnyAsync(t => t.Name.ToLower() == normalized);


    }

}