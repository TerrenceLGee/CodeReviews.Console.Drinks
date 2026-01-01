using DrinksInfo.TerrenceLGee.OptionEnums;
using DrinksInfo.TerrenceLGee.Services;
using DrinksInfo.TerrenceLGee.Tests.Extensions;
using DrinksInfo.TerrenceLGee.Tests.JsonResponses;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace DrinksInfo.TerrenceLGee.Tests;

public class DrinkServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockClientFactory;
    private readonly Mock<ILogger<DrinkService>> _mockLogger;

    public DrinkServiceTests()
    {
        _mockClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<DrinkService>>();
    }

    [Fact]
    public async Task GetDrinksAsync_ReturnsListOfDrinksByCategoryCocktail_WhenConnectionSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.CategoryQuery)
            .ReturnsHttpResponseAsync(CategoryCocktailReponse.GetCategoryCocktailResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetDrinksAsync(DrinkSearchOptions.Category, Queries.CategoryName);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("155 Belmont", result[0].DrinkName);
        Assert.Equal("178355", result[^1].DrinkId);
    }

    [Fact]
    public async Task GetDrinksAsync_ReturnsEmptyList_WhenAPI_ReturnsUnavailable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.CategoryQuery)
            .ReturnsHttpResponseAsync(CategoryCocktailReponse.GetCategoryCocktailResponse, HttpStatusCode.ServiceUnavailable);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetDrinksAsync(DrinkSearchOptions.Category, Queries.CategoryName);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetDrinksAsync_ReturnsEmptyList_WhenAPI_IsUnreachable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.CategoryQuery)
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetDrinksAsync(DrinkSearchOptions.Category, Queries.CategoryName);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetDrinkDetailAsync_ReturnsDrinkDetails_WhenSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();
        var drinkId = "11007";

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, drinkId)
            .ReturnsHttpResponseAsync(DrinkByIdResponse.GetDrinkByIdResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetDrinkDetailAsync(drinkId);

        Assert.NotNull(result);
        Assert.Equal(drinkId, result.DrinkId);
        Assert.Equal("Margarita", result.DrinkName);
        Assert.Equal("Salt", result.Ingredient4);
        Assert.Contains("Rub the rim of", result.Instructions);
    }

    [Fact]
    public async Task GetDrinkDetailAsync_ReturnsNull_WhenAPI_ReturnsUnavailable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();
        var drinkId = "11007";

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, drinkId)
            .ReturnsHttpResponseAsync(DrinkByIdResponse.GetDrinkByIdResponse, HttpStatusCode.ServiceUnavailable);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetDrinkDetailAsync(drinkId);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetDrinkDetailAsync_ReturnsNull_WhenAPI_IsUnreachable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();
        var drinkId = "11007";

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, drinkId)
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetDrinkDetailAsync(drinkId);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetRandomDrinkDetailAsync_APIReturnsARandomDrink_WhenSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.RandomCategoryQuery)
            .ReturnsHttpResponseAsync(RandomDrinkResponse.GetRandomDrinkResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetRandomDrinkDetailAsync();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetRandomDrinkDetailAsync_ReturnsNull_WhenAPIReturnsUnavailable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.RandomCategoryQuery)
            .ReturnsHttpResponseAsync(RandomDrinkResponse.GetRandomDrinkResponse, HttpStatusCode.ServiceUnavailable);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetRandomDrinkDetailAsync();

        Assert.Null(result);
    }

    [Fact]
    public async Task GetRandomDrinkDetailAsync_ReturnsNull_WhenAPI_IsUnreachable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.RandomCategoryQuery)
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetRandomDrinkDetailAsync();

        Assert.Null(result);
    }

    [Fact]
    public async Task GetDrinksAsync_ReturnsListOfDrinksByGlassCocktailFlute_WhenConnectionSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.GlassQuery)
            .ReturnsHttpResponseAsync(GlassesChampagneFluteResponse.GetGlassesChampagneFluteResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetDrinksAsync(DrinkSearchOptions.Glass, Queries.GlassName);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("17224", result[0].DrinkId);
        Assert.Equal("Arise My Love", result[1].DrinkName);
    }

    [Fact]
    public async Task GetDrinksAsync_ReturnsListOfDrinkByIngredientGin_WhenConnectionSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.IngredientQuery)
            .ReturnsHttpResponseAsync(IngredientGinResponse.GetIngredientGinResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetDrinksAsync(DrinkSearchOptions.Ingredient, Queries.IngredientName);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("Abbey Cocktail", result[3].DrinkName);
        Assert.Equal("12420", result[^1].DrinkId);
    }

    [Fact]
    public async Task GetDrinksAsync_ReturnsListOfDrinksByAlcoholicNonAlcoholic_WhenConnectionSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.AlcoholicQuery)
            .ReturnsHttpResponseAsync(AlcoholicNonAlcoholicResponse.GetAlcoholicNonAlcoholicResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var drinkService = new DrinkService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await drinkService.GetDrinksAsync(DrinkSearchOptions.Alcoholic, Queries.AlcoholicName);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("Afterglow", result[0].DrinkName);
        Assert.Equal("12862", result[2].DrinkId);
    }
}
