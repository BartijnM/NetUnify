using System.Text.Json;
using NetTopologySuite.Geometries;
using NetUnify.Api.Mapping;
using NetUnify.Api.Models;
using Xunit;

namespace NetUnify.Api.Tests;

public sealed class AssetGeoJsonMapperTests
{
    private static JsonElement Serialize(IReadOnlyList<Asset> assets)
    {
        var collection =
            AssetGeoJsonMapper.CreateFeatureCollection(assets);

        return JsonSerializer.SerializeToElement(
            collection,
            new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    [Fact]
    public void EmptySelectionProducesEmptyFeatureCollection()
    {
        var json = Serialize(Array.Empty<Asset>());

        Assert.Equal(
            "FeatureCollection",
            json.GetProperty("type").GetString());

        var features = json.GetProperty("features");

        Assert.Equal(JsonValueKind.Array, features.ValueKind);
        Assert.Equal(0, features.GetArrayLength());
    }

    [Fact]
    public void AssetProducesFeatureWithSourcePropertiesAndWgs84Paths()
    {
        var factory = NetTopologySuite.NtsGeometryServices.Instance
            .CreateGeometryFactory(srid: 28992);

        var geometry = factory.CreateMultiLineString(
        [
            factory.CreateLineString(
            [
                new Coordinate(210650, 467800),
                new Coordinate(210750, 467800)
            ]),
            factory.CreateLineString(
            [
                new Coordinate(210650, 467900),
                new Coordinate(210650, 468000)
            ])
        ]);

        var asset = new Asset(
            AssetId: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            Operator: "Liander",
            SourceDataset: "Liander_Open_Data_Elektra/112",
            SourceRecordId: "123",
            Discipline: "electricity",
            AssetType: "cable",
            VoltageClass: "LV",
            VoltageKv: null,
            Material: null,
            Geometry: geometry,
            JobId: Guid.Parse("00000000-0000-0000-0000-000000000010"));

        var json = Serialize(new[] { asset });

        Assert.Equal(
            "FeatureCollection",
            json.GetProperty("type").GetString());

        var features = json.GetProperty("features");
        Assert.Equal(1, features.GetArrayLength());

        var feature = features[0];

        Assert.Equal("Feature", feature.GetProperty("type").GetString());
        Assert.Equal(
            asset.AssetId.ToString(),
            feature.GetProperty("id").GetString());

        var properties = feature.GetProperty("properties");

        Assert.Equal(
            "123", properties.GetProperty("sourceRecordId").GetString());
        Assert.Equal(
            "Liander", properties.GetProperty("operator").GetString());
        Assert.Equal(
            "Liander_Open_Data_Elektra/112",
            properties.GetProperty("sourceDataset").GetString());

        var outputGeometry = feature.GetProperty("geometry");

        Assert.Equal(
            "MultiLineString",
            outputGeometry.GetProperty("type").GetString());

        var paths = outputGeometry.GetProperty("coordinates");

        Assert.Equal(2, paths.GetArrayLength());
        Assert.Equal(2, paths[0].GetArrayLength());
        Assert.Equal(2, paths[1].GetArrayLength());

        var firstPoint = paths[0][0];

        Assert.Equal(2, firstPoint.GetArrayLength());

        const double tolerance = 0.00001;

        Assert.InRange(
            firstPoint[0].GetDouble(),
            6.201141586 - tolerance,
            6.201141586 + tolerance);

        Assert.InRange(
            firstPoint[1].GetDouble(),
            52.195506749 - tolerance,
            52.195506749 + tolerance);

        Assert.Equal(28992, geometry.SRID);
        Assert.Equal(210650d, geometry.GetGeometryN(0).Coordinate.X);
        Assert.Equal(467800d, geometry.GetGeometryN(0).Coordinate.Y);
    }
}