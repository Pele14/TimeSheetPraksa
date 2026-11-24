using System.Text.RegularExpressions;
using Timesheet.Domain.Entities;
using Timesheet.Infrastructure.DBEntities;

namespace TimeSheet.Infrastructure.Mappers;



public static class CategoryMapper
{
    public static Category MapToEntity(this CategoryModel category)

        => new Category
        {
            Id = category.Id,
            Name = category.Name


        };


    public static CategoryModel MapToDomain(this Category category)

        => new CategoryModel(category.Id, category.Name);

    public static void UpdateEntity(this Category category, CategoryModel entity)
    {
        category.Name = entity.Name;
    }


}

