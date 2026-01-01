using DrinksInfo.TerrenceLGee.Services;
using DrinksInfo.TerrenceLGee.Tests.Extensions;
using DrinksInfo.TerrenceLGee.Tests.JsonResponses;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace DrinksInfo.TerrenceLGee.Tests;

public class IngredientDetailServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockClientFactory;
    private readonly Mock<ILogger<IngredientDetailService>> _mockLogger;

    public IngredientDetailServiceTests()
    {
        _mockClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<IngredientDetailService>>();
    }

    [Fact]
    public async Task GetIngredientDetailAsync_API_ReturnsIngredient_WhenConnectionSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.IngredientDetailQuery)
            .ReturnsHttpResponseAsync(IngredientDetailResponse.GetIngredientDetailResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var ingredientDetailService = new IngredientDetailService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await ingredientDetailService.GetIngredientDetailAsync(Queries.IngredientName);

        Assert.NotNull(result);
        Assert.Equal("2", result.IngredientId);
        Assert.Equal("40", result.AlcoholByVolume);
        Assert.Contains("distilled alcoholic drink", result.Description);
    }

    [Fact]
    public async Task GetIngredientDetailAsync_ReturnsNull_WhenAPI_ReturnsUnavailable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.IngredientDetailQuery)
            .ReturnsHttpResponseAsync(IngredientDetailResponse.GetIngredientDetailResponse, HttpStatusCode.ServiceUnavailable);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var ingredientDetailService = new IngredientDetailService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await ingredientDetailService.GetIngredientDetailAsync(Queries.IngredientName);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetIngredientDetailAsync_ReturnsNull_WhenAPI_IsUnreachable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.IngredientDetailQuery)
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var ingredientDetailService = new IngredientDetailService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await ingredientDetailService.GetIngredientDetailAsync(Queries.IngredientName);

        Assert.Null(result);
    }
}
