using System.Net;
using System.Text.Json;
using Infrastructure.Binance;
using Microsoft.Extensions.Options;

namespace Integration.Tests.BrokerController.Stubs;

public class BinanceStubHttpMessageHandler : HttpMessageHandler
{
    private readonly BinanceApiSettings _binanceApiSettings;
    private readonly MockData _mockData;

    public BinanceStubHttpMessageHandler(IOptions<BinanceApiSettings> binanceApiSettings)
    {
        var jsonData = File.ReadAllText("MockApis/binanceMockData.json");
        _mockData = JsonSerializer.Deserialize<MockData>(jsonData);
        _binanceApiSettings = binanceApiSettings.Value;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(GetMockResponse(request))
        };
        return Task.FromResult(response);
    }

    private string GetMockResponse(HttpRequestMessage request)
    {
        return request.RequestUri?.AbsoluteUri switch
        {
            { } p when p.Contains(_binanceApiSettings.CurrentPriceUrl) => _mockData.CurrentPriceResponse,
            { } p when p.Contains(_binanceApiSettings.HistoricalPriceUrl) => _mockData.HistoricalPriceResponse,
            _ => throw new Exception($"Unexpected path: {request.RequestUri?.AbsolutePath}")
        };
    }
}

public class MockData
{
    public required string CurrentPriceResponse { get; set; }
    public required string HistoricalPriceResponse { get; set; }
}