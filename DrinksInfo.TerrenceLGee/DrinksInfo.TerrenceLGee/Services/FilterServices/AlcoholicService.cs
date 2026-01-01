using DrinksInfo.TerrenceLGee.Data;
using DrinksInfo.TerrenceLGee.Models.FilterModels.AlcoholicFilterModels;
using DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace DrinksInfo.TerrenceLGee.Services.FilterServices;

public class AlcoholicService : IAlcoholicService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<AlcoholicService> _logger;
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";

    public AlcoholicService(
        IHttpClientFactory clientFactory,
        ILogger<AlcoholicService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<List<Alcoholic>> GetAlcoholicsAsync()
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var result = await httpClient.GetFromJsonAsync<AlcoholicRoot>(Urls.AlcoholicListQuery);
            return result?.Alcoholics ?? [];
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AlcoholicService)}\n" +
                            $"Method: {nameof(GetAlcoholicsAsync)}\n" +
                            $"There was an API error retrieving the alcoholic categories: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        } 
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AlcoholicService)}\n" +
                $"Method: {nameof(GetAlcoholicsAsync)}\n" +
                $"There was an unexpected error retrieving the alcoholic categories: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }
}
