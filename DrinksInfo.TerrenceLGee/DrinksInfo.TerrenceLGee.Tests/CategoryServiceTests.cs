using DrinksInfo.TerrenceLGee.Services.FilterServices;
using DrinksInfo.TerrenceLGee.Tests.Extensions;
using DrinksInfo.TerrenceLGee.Tests.JsonResponses;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace DrinksInfo.TerrenceLGee.Tests;

public class CategoryServiceTests
{
    private readonly Mock<IHttpClientFactory> _mockClientFactory;
    private readonly Mock<ILogger<CategoryService>> _mockLogger;

    public CategoryServiceTests()
    {
        _mockClientFactory = new Mock<IHttpClientFactory>();
        _mockLogger = new Mock<ILogger<CategoryService>>();
    }

    [Fact]
    public async Task GetCategoriesAsync_API_ReturnsAListOfCategories_WhenConnectionSuccessful()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.CategoriesQuery)
            .ReturnsHttpResponseAsync(CategoryResponse.GetCategoriesResponse, HttpStatusCode.OK);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var categoryService = new CategoryService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await categoryService.GetCategoriesAsync();

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal("Beer", result[0].CategoryName);
        Assert.Equal("Shot", result[^2].CategoryName);
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsAnEmptyList_WhenAPI_ReturnsUnavailable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.CategoriesQuery)
            .ReturnsHttpResponseAsync(CategoryResponse.GetCategoriesResponse, HttpStatusCode.ServiceUnavailable);

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var categoryService = new CategoryService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await categoryService.GetCategoriesAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsAnEmptyList_WhenAPI_IsUnreachable()
    {
        var httpMessageHandler = new Mock<HttpMessageHandler>();

        httpMessageHandler
            .SetupSendAsync(HttpMethod.Get, Queries.CategoriesQuery)
            .ThrowsAsync(new HttpRequestException());

        var httpClient = new HttpClient(httpMessageHandler.Object)
        {
            BaseAddress = new Uri(Queries.MockUrl)
        };

        _mockClientFactory
            .Setup(_ => _.CreateClient(Queries.ClientName))
            .Returns(httpClient);

        var categoryService = new CategoryService(_mockClientFactory.Object, _mockLogger.Object);

        var result = await categoryService.GetCategoriesAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
