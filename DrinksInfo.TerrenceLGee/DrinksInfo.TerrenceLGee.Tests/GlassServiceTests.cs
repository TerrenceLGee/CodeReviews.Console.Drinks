using DrinksInfo.TerrenceLGee.Services.FilterServices;
using DrinksInfo.TerrenceLGee.Tests.Extensions;
using DrinksInfo.TerrenceLGee.Tests.JsonResponses;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace DrinksInfo.TerrenceLGee.Tests;

public class GlassServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockClientFactory;
    private readonly Mock<ILogger<GlassService>> _mockLogger;

    public GlassServiceTests()
    {
        _mockClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<GlassService>>();
    }

    [Fact]
    public async Task GetGlassesAsync_API_ReturnsListOfGlasses_WhenConnectionSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.GlassesQuery)
            .ReturnsHttpResponseAsync(GlassesResponse.GetGlassResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var glassService = new GlassService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await glassService.GetGlassesAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("Balloon Glass", result[0].GlassName);
        Assert.Equal("Cocktail glass", result[6].GlassName);
    }

    [Fact]
    public async Task GetGlassesAsync_ReturnsEmptyList_WhenAPI_ReturnsUnavailable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.GlassesQuery)
            .ReturnsHttpResponseAsync(GlassesResponse.GetGlassResponse, HttpStatusCode.ServiceUnavailable);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var glassService = new GlassService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await glassService.GetGlassesAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetGlassesAsync_ReturnsEmptyList_WhenAPI_IsUnreachable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.GlassesQuery)
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var glassService = new GlassService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await glassService.GetGlassesAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
