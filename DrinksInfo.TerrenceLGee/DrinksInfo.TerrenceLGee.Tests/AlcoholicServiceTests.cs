using DrinksInfo.TerrenceLGee.Services.FilterServices;
using DrinksInfo.TerrenceLGee.Tests.Extensions;
using DrinksInfo.TerrenceLGee.Tests.JsonResponses;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace DrinksInfo.TerrenceLGee.Tests;

public class AlcoholicServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockClientFactory;
    private readonly Mock<ILogger<AlcoholicService>> _mockLogger;

    public AlcoholicServiceTests()
    {
        _mockClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<AlcoholicService>>();
    }

    [Fact]
    public async Task GetAlcoholicsAsync_API_ReturnsListOfIngredients_WhenConnectionSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.AlcoholicsQuery)
            .ReturnsHttpResponseAsync(AlcoholicResponse.GetAlcoholicResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var alcoholicService = new AlcoholicService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await alcoholicService.GetAlcoholicsAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("Alcoholic", result[0].AlcoholicName);
        Assert.Equal("Non alcoholic", result[1].AlcoholicName);
        Assert.Equal("Optional alcohol", result[2].AlcoholicName);
    }

    [Fact]
    public async Task GetAlcoholicsAsync_ReturnsEmptyList_WhenAPI_ReturnsUnavailable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.AlcoholicsQuery)
            .ReturnsHttpResponseAsync(AlcoholicResponse.GetAlcoholicResponse, HttpStatusCode.ServiceUnavailable);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var alcoholicService = new AlcoholicService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await alcoholicService.GetAlcoholicsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetIngredientsAsync_ReturnsEmptyList_WhenAPI_IsUnreachable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.AlcoholicsQuery)
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var alcoholicService = new AlcoholicService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await alcoholicService.GetAlcoholicsAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
