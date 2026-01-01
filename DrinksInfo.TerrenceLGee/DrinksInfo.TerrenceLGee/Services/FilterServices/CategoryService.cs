using DrinksInfo.TerrenceLGee.Data;
using DrinksInfo.TerrenceLGee.Models.FilterModels.CategoryFilterModels;
using DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace DrinksInfo.TerrenceLGee.Services.FilterServices;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<CategoryService> _logger;
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";

    public CategoryService(
        IHttpClientFactory clientFactory,
        ILogger<CategoryService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var response = await httpClient.GetFromJsonAsync<CategoryRoot>(Urls.CategoryListQuery);
            return response?.Categories ?? [];
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                            $"Method: {nameof(GetCategoriesAsync)}\n" +
                            $"There was an API error retrieving the drink categories: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryService)}\n" +
                            $"Method: {nameof(GetCategoriesAsync)}\n" +
                            $"There was an unexpected error retrieving the drink categories: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }
}
