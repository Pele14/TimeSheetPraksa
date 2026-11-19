using System.Text.RegularExpressions;
using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class CategoryMapper
{
    public static Category MapToEntity(this CategoryEntity category)

        => new Category
        {
            Id = category.Id,
            Name = category.Name


        };


    public static CategoryEntity MapToDomain(this Category category)

        => new CategoryEntity(category.Id, category.Name);

    public static void UpdateEntity(this Category category, CategoryEntity entity)
    {
        category.Name = entity.Name;
    }


}

