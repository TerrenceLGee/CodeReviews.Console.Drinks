using DrinksInfo.TerrenceLGee.Data;
using DrinksInfo.TerrenceLGee.Models.FilterModels.IngredientFilterModels;
using DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace DrinksInfo.TerrenceLGee.Services.FilterServices;

public class IngredientService : IIngredientService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<IngredientService> _logger;
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";

    public IngredientService(
        IHttpClientFactory clientFactory,
        ILogger<IngredientService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<List<Ingredient>> GetIngredientsAsync()
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var result = await httpClient.GetFromJsonAsync<IngredientRoot>(Urls.IngredientsListQuery);
            return result?.Ingredients ?? [];
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(IngredientService)}\n" +
                $"Method: {nameof(GetIngredientsAsync)}\n" +
                $"There was an API error retrieving the drink ingredients: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(IngredientService)}\n" +
                $"Method: {nameof(GetIngredientsAsync)}\n" +
                $"There was an unexpected error retrieving the drink ingredients: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }
}
