using CaloryCalcApp.Application.DTOs.Search;

namespace CaloryCalcApp.Application.Services.Interfaces
{
    public interface ISearchService
    {
        Task<SearchResultDto> SearchAsync(string searchText, string searchType);
    }
}
