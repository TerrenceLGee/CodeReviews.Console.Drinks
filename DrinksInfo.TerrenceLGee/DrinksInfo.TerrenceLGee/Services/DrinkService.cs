using DrinksInfo.TerrenceLGee.Data;
using DrinksInfo.TerrenceLGee.Models.DrinkModels;
using DrinksInfo.TerrenceLGee.OptionEnums;
using DrinksInfo.TerrenceLGee.Services.Interfaces.DrinkServiceInterfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace DrinksInfo.TerrenceLGee.Services;

public class DrinkService : IDrinkService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<DrinkService> _logger;
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";

    public DrinkService(
        IHttpClientFactory clientFactory,
        ILogger<DrinkService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<List<DrinkSummary>> GetDrinksAsync(DrinkSearchOptions option, string searchTerm)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = GetDrinksUrl(option, searchTerm);
            var result = await httpClient.GetFromJsonAsync<DrinkSummaryRoot>(url);
            return result?.DrinkSummaries ?? [];
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(DrinkService)}\n" +
                            $"Method: {nameof(GetDrinksAsync)}\n" +
                            $"There was an API error retrieving drinks by {searchTerm}: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(DrinkService)}\n" +
                            $"Method: {nameof(GetDrinksAsync)}\n" +
                            $"There was an unexpected error retrieving drinks by {searchTerm}: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }

    public async Task<DrinkDetail?> GetDrinkDetailAsync(string drinkId)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var result = await httpClient.GetFromJsonAsync<DrinkDetailRoot>($"{Urls.LookupDrinkDetailsByDrinkIdQuery}{drinkId}");
            return result?.DrinkDetails.FirstOrDefault();
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(DrinkService)}\n" +
                            $"Method: {nameof(GetDrinkDetailAsync)}\n" +
                            $"There was an API error retrieving drink {drinkId}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(DrinkService)}\n" +
                            $"Method: {nameof(GetDrinkDetailAsync)}\n" +
                            $"There was an unexpected error retrieving drink {drinkId}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<DrinkDetail?> GetRandomDrinkDetailAsync()
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var result = await httpClient.GetFromJsonAsync<DrinkDetailRoot>($"{Urls.GetARandomDrinkQuery}");
            return result?.DrinkDetails.FirstOrDefault();
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(DrinkService)}\n" +
                $"Method: {nameof(GetRandomDrinkDetailAsync)}\n" +
                $"There was an API error retrieving a randomd drink: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(DrinkService)}\n" +
                $"Method: {nameof(GetRandomDrinkDetailAsync)}\n" +
                $"There was an unexpected error retrieving a random drink: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    private static string GetDrinksUrl(
        DrinkSearchOptions option, 
        string searchTerm)
    {
        var url = option switch
        {
            DrinkSearchOptions.Category => Urls.FilterDrinksByCategoryQuery,
            DrinkSearchOptions.Ingredient => Urls.FilterDrinksByIngredientQuery,
            DrinkSearchOptions.Alcoholic => Urls.FilterDrinksByAlcoholicQuery,
            DrinkSearchOptions.Glass => Urls.FilterDrinksByGlassQuery,
            DrinkSearchOptions.FirstLetter => Urls.SearchDrinksByFirstLetterQuery,
            DrinkSearchOptions.SearchTerm => Urls.SearchDrinksByLettersQuery,
            _ => string.Empty
        };

        return $"{url}{searchTerm}";      
    }
}
