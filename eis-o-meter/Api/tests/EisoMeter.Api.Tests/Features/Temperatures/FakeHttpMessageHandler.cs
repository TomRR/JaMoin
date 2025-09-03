using System.Net;

namespace EisoMeter.Api.Tests.Features.Temperatures;

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly string _response;

    public FakeHttpMessageHandler(string response)
    {
        _response = response;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var message = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_response)
        };
        return Task.FromResult(message);
    }
}