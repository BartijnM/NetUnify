using System.Net;
using System.Text;
using System.Text.Json;
using NetUnify.Api.Connectors;
using Xunit;

namespace NetUnify.Api.Tests;

public sealed class LianderConnectorTests
{
    [Theory]
    [InlineData("""{"wkid":28992}""")]
    [InlineData("""{"wkid":28992,"latestWkid":28992}""")]
    public async Task RdMetadataReturnsUsableFeatures(
        string spatialReference)
    {
        var json = $$"""
        {
            "spatialReference": {{spatialReference}},
            "features": [
                { "attributes": { "OBJECTID": 123 } }
            ]
        }
        """;

        using var client = new HttpClient(new FixedResponseHandler(json));
        var connector = new LianderConnector(client);

        var features = await connector.GetFeaturesAsync();

        // Ook na het opruimen van het JsonDocument blijven features leesbaar.
        Assert.Equal(JsonValueKind.Array, features.ValueKind);
        Assert.Equal(1, features.GetArrayLength());
        Assert.Equal(
            123,
            features[0].GetProperty("attributes")
                .GetProperty("OBJECTID").GetInt32());
    }

    [Theory]
    [InlineData("""{}""")]
    [InlineData("""{"spatialReference":null}""")]
    [InlineData("""{"spatialReference":[]}""")]
    [InlineData("""{"spatialReference":{}}""")]
    [InlineData("""{"spatialReference":{"wkid":null}}""")]
    [InlineData("""{"spatialReference":{"wkid":"28992"}}""")]
    [InlineData("""{"spatialReference":{"wkid":4326}}""")]
    [InlineData("""{"spatialReference":{"wkid":2147483648}}""")]
    [InlineData("""{"spatialReference":{"wkid":28992,"latestWkid":4326}}""")]
    [InlineData("""{"spatialReference":{"wkid":28992,"latestWkid":null}}""")]
    [InlineData("""{"spatialReference":{"wkid":28992,"latestWkid":"28992"}}""")]
    [InlineData("""{"spatialReference":{"wkid":28992,"latestWkid":2147483648}}""")]
    public async Task InvalidCrsMetadataStopsTheRequest(string json)
    {
        using var client = new HttpClient(new FixedResponseHandler(json));
        var connector = new LianderConnector(client);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => connector.GetFeaturesAsync());

        Assert.StartsWith("Het Liander-antwoord", exception.Message);
    }

    private sealed class FixedResponseHandler(string json)
        : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    json, Encoding.UTF8, "application/json")
            });
        }
    }
}