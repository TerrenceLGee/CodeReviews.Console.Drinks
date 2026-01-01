using DrinksInfo.TerrenceLGee.Services.FilterServices;
using DrinksInfo.TerrenceLGee.Tests.Extensions;
using DrinksInfo.TerrenceLGee.Tests.JsonResponses;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace DrinksInfo.TerrenceLGee.Tests;

public class IngredientServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockClientFactory;
    private readonly Mock<ILogger<IngredientService>> _mockLogger;

    public IngredientServiceTests()
    {
        _mockClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<IngredientService>>();
    }

    [Fact]
    public async Task GetIngredientsAsync_API_ReturnsListOfIngredients_WhenConnectionSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.IngredientsQuery)
            .ReturnsHttpResponseAsync(IngredientsResponse.GetIngredientResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var ingredientService = new IngredientService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await ingredientService.GetIngredientsAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("151 proof rum", result[0].IngredientName);
        Assert.Equal("Kiwi", result[^1].IngredientName);
    }

    [Fact]
    public async Task GetIngredientsAsync_ReturnsEmptyList_WhenAPI_ReturnsUnavailable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.IngredientsQuery)
            .ReturnsHttpResponseAsync(IngredientsResponse.GetIngredientResponse, HttpStatusCode.ServiceUnavailable);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var ingredientService = new IngredientService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await ingredientService.GetIngredientsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetIngredientsAsync_ReturnsEmptyList_WhenAPI_IsUnreachable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.IngredientsQuery)
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var ingredientService = new IngredientService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await ingredientService.GetIngredientsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
