using MediatR;
using TimeSheet.Application.Category;
using TimeSheet.Application.Client.Queries.GetCategories;
using TimeSheet.Application.Responses.CategoryResponses;
using TimeSheet.Application.Responses.ClientResponses;
using TimeSheet.Application.Responses.TeamMemberResponses;
using TimeSheet.Core.Repositories;

namespace TimeSheet.Application.Client.Queries.GetCategories;



public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, CategoriesResponse>
{
    private readonly ICategoryRepository CategoryRepository;

    public GetCategoriesHandler(ICategoryRepository categoryRepository)
    {
        CategoryRepository = categoryRepository;
    }
    public async Task<CategoriesResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var members = await CategoryRepository.GetAllAsync();

        return new CategoriesResponse
        {
           Categories= members.Select(c=>new CategoryReponse{Id=c.Id, Name=c.Name}).ToList()
        };
    }
}