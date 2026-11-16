namespace TimeSheet.Infrastructure.Persistence;


public interface ISoftDelitable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}