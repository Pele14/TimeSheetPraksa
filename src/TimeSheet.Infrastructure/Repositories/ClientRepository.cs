using TimeSheet.Infrastructure.Persistence;
using TimeSheet.Core.Repositories;
using Timesheet.Domain.Entities;
using TimeSheet.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;
namespace TimeSheet.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClientEntity?> GetByIdAsync(Guid id)
    {
        var entity = await _context.Clients.FindAsync(id);
        return entity?.MapToDomain();
    }

    public async Task<IEnumerable<ClientEntity>> GetAllAsync()
    {
        var entities = await _context.Clients.OrderBy(c=>c.Name).ToListAsync();
        return entities.Select(e => e.MapToDomain()).ToList();
    }

    public async Task AddAsync(ClientEntity client)
    {
        await _context.Clients.AddAsync(client.MapToEntity());
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ClientEntity client)
    {
        var entity = await _context.Clients.FindAsync(client.Id)
          ?? throw new KeyNotFoundException($"Član tima {client.Id} ne postoji.");

        client.UpdateEntity(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
        else
            throw new KeyNotFoundException("Id ne posoji");
    }
    public async Task<bool> ExistsByNameAsync(string name)
    {
        var normalized = name.Trim().ToLower();
        return await _context.Clients.AnyAsync(t => t.Name == normalized);


    }
    public async Task<bool> HasProjectAsync(Guid clientid)
    {
        return await _context.Projects.AnyAsync(p=>p.ClientId==clientid);    
    }

}