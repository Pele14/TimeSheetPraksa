

namespace Timesheet.Domain.Entities;

public class CategoryModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }



    public CategoryModel(Guid id, string name)
    {

        Id = id;
        Name = name;
    }


}
