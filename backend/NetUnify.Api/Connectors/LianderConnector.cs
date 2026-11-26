using System.Text.Json;

namespace NetUnify.Api.Connectors;

public sealed class LianderConnector(HttpClient httpClient)
{
    public async Task<JsonElement> GetFeaturesAsync(
        CancellationToken cancellationToken = default)
    {
        const string url =
            "https://services1.arcgis.com/v6W5HAVrpgSg3vts/arcgis/rest/services/" +
            "Liander_Open_Data_Elektra/FeatureServer/112/query" +
            "?where=1%3D1" +
            "&geometry=210650%2C467800%2C210850%2C468000" +
            "&geometryType=esriGeometryEnvelope" +
            "&inSR=28992&outSR=28992" +
            "&spatialRel=esriSpatialRelIntersects" +
            "&outFields=*&returnGeometry=true" +
            "&orderByFields=OBJECTID%20ASC" +
            "&resultRecordCount=100&f=json";

        using var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync(
            cancellationToken);
        using var document = await JsonDocument.ParseAsync(
            stream, cancellationToken: cancellationToken);

        var root = document.RootElement;

        if (root.TryGetProperty("error", out _))
        {
            throw new InvalidOperationException(
                "Liander heeft een foutantwoord teruggegeven.");
        }

        if (root.TryGetProperty("exceededTransferLimit", out var exceeded)
            && exceeded.GetBoolean())
        {
            throw new InvalidOperationException(
                "De Liander-selectie is afgekapt. Paginering is nodig.");
        }

        if (!root.TryGetProperty("spatialReference", out var spatialReference)
            || spatialReference.ValueKind != JsonValueKind.Object
            || !spatialReference.TryGetProperty("wkid", out var wkid)
            || wkid.ValueKind != JsonValueKind.Number
            || !wkid.TryGetInt32(out var wkidValue)
            || wkidValue != 28992)
        {
            throw new InvalidOperationException(
                "Het Liander-antwoord moet spatialReference.wkid 28992 bevatten.");
        }

        if (spatialReference.TryGetProperty("latestWkid", out var latestWkid)
            && (latestWkid.ValueKind != JsonValueKind.Number
                || !latestWkid.TryGetInt32(out var latestWkidValue)
                || latestWkidValue != 28992))
        {
            throw new InvalidOperationException(
                "Het Liander-antwoord bevat een ongeldige of afwijkende latestWkid.");
        }

        var features = root.GetProperty("features");

        if (features.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException(
                "Het Liander-antwoord bevat geen geldige features-array.");
        }

        return features.Clone();
    }
}