using MediatR;
using TimeSheet.Application.Responses.CategoryResponses;

namespace TimeSheet.Application.Category;


public record GetCategoriesQuery: IRequest<CategoriesResponse>;