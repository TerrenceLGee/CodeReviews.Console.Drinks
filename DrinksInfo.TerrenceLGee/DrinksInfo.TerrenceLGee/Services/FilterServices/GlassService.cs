using DrinksInfo.TerrenceLGee.Data;
using DrinksInfo.TerrenceLGee.Models.FilterModels.GlassFilterModels;
using DrinksInfo.TerrenceLGee.Services.Interfaces.FilterServiceInterfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace DrinksInfo.TerrenceLGee.Services.FilterServices;

public class GlassService : IGlassService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<GlassService> _logger;
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";

    public GlassService(
        IHttpClientFactory clientFactory,
        ILogger<GlassService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<List<Glass>> GetGlassesAsync()
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var result = await httpClient.GetFromJsonAsync<GlassRoot>(Urls.GlassesListQuery);
            return result?.Glasses ?? [];
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(GlassService)}\n" +
                            $"Method: {nameof(GetGlassesAsync)}\n" +
                            $"There was an API error retrieving the glass categories: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(GlassService)}\n" +
                            $"Method: {nameof(GetGlassesAsync)}\n" +
                            $"There was an unexpected error retrieving the glass categories: {ex.Message}\n";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }
}
