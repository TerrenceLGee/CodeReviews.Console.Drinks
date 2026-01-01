using DrinksInfo.TerrenceLGee.Data;
using DrinksInfo.TerrenceLGee.Models.IngredientModels;
using DrinksInfo.TerrenceLGee.Services.Interfaces.IngredientServiceInterfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace DrinksInfo.TerrenceLGee.Services;

public class IngredientDetailService : IIngredientDetailService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<IngredientDetailService> _logger;
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";

    public IngredientDetailService(
        IHttpClientFactory clientFactory,
        ILogger<IngredientDetailService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<IngredientDetail?> GetIngredientDetailAsync(string ingredientName)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var result = await httpClient.GetFromJsonAsync<IngredientDetailRoot>($"{Urls.LookupIngredientByNameQuery}{ingredientName}");
            return result?.IngredientDetails.FirstOrDefault();
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(IngredientDetailService)}\n" +
                $"Method: {nameof(GetIngredientDetailAsync)}\n" +
                $"There was an API error retrieving the details for the ingredient {ingredientName}: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(IngredientDetailService)}\n" +
                $"Method: {nameof(GetIngredientDetailAsync)}\n" +
                $"There was an unexpected error retrieving the details for the ingredient {ingredientName}: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }
}
