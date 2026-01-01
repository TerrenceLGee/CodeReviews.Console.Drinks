using System.Net;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;

namespace DrinksInfo.TerrenceLGee.Tests.Extensions;

public static class MoqExtensions
{
    public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupSendAsync(
        this Mock<HttpMessageHandler> handler, HttpMethod requestMethod, string expectedRelativeUrl)
    {
        return handler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
            ItExpr.Is<HttpRequestMessage>(r =>
            r.Method == requestMethod && 
            r.RequestUri != null &&
            r.RequestUri.PathAndQuery.EndsWith(expectedRelativeUrl)),
            ItExpr.IsAny<CancellationToken>());
    }

    public static IReturnsResult<HttpMessageHandler> ReturnsHttpResponseAsync(
        this ISetup<HttpMessageHandler, Task<HttpResponseMessage>> moqSetup, 
        string? responseBody, HttpStatusCode responseCode)
    {
        var stringContent = new StringContent(responseBody ?? string.Empty);

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = responseCode,
            Content = stringContent
        };

        return moqSetup.ReturnsAsync(responseMessage);
    }
}
