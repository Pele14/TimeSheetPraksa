using Timesheet.Domain.Entities;

namespace TimeSheet.Core.Repositories;

public interface ITimeEntryRepository
{
    Task<TimeEntryModel?>GetByIdAsync(Guid id);

    Task AddAsync(TimeEntryModel timeEntry);
    Task UpdateAsync(TimeEntryModel timeEntry);
    Task DeleteAsync(Guid id);

}
