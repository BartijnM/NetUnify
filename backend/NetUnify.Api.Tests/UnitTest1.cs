using System.Text.Json;
using NetTopologySuite.Geometries;
using NetUnify.Api.Models;
using NetUnify.Api.Services;
using Xunit;

namespace NetUnify.Api.Tests;

public sealed class LianderImportProcessorTests
{
    private static readonly Guid JobId =
        Guid.Parse("00000000-0000-0000-0000-000000000010");

    [Fact]
    // Test die controleert of een geldig record correct wordt verwerkt en dat de mapping en paden behouden blijven.
    public void ValidRecordPreservesMappingAndPaths()
    {
        var result = Process("""
        [
            {
                "attributes": { "OBJECTID": 1 },
                "geometry": {
                    "paths": [
                        [[210650, 467800], [210750, 467800]],
                        [[210700, 467900], [210800, 467900]]
                    ]
                }
            }
        ]
        """);

        var asset = Assert.Single(result.Assets);

        Assert.Equal(1, result.Read);
        Assert.Empty(result.Quarantine);
        Assert.NotEqual(Guid.Empty, asset.AssetId);
        Assert.Equal(JobId, asset.JobId);
        Assert.Equal("1", asset.SourceRecordId);
        Assert.Equal("Liander", asset.Operator);
        Assert.Equal("Liander_Open_Data_Elektra/112", asset.SourceDataset);
        Assert.Equal("electricity", asset.Discipline);
        Assert.Equal("cable", asset.AssetType);
        Assert.Equal("LV", asset.VoltageClass);
        Assert.Null(asset.VoltageKv);
        Assert.Null(asset.Material);
        Assert.Equal(28992, asset.Geometry.SRID);
        Assert.Equal(2, asset.Geometry.NumGeometries);

        var first = Assert.IsType<LineString>(
            asset.Geometry.GetGeometryN(0));
        var second = Assert.IsType<LineString>(
            asset.Geometry.GetGeometryN(1));

        Assert.Equal(2, first.NumPoints);
        Assert.Equal(2, second.NumPoints);
        Assert.Equal(210650d, first.GetCoordinateN(0).X);
        Assert.Equal(467800d, first.GetCoordinateN(0).Y);
        Assert.Equal(210750d, first.GetCoordinateN(1).X);
        Assert.Equal(467800d, first.GetCoordinateN(1).Y);
        Assert.Equal(210700d, second.GetCoordinateN(0).X);
        Assert.Equal(467900d, second.GetCoordinateN(0).Y);
        Assert.Equal(210800d, second.GetCoordinateN(1).X);
        Assert.Equal(467900d, second.GetCoordinateN(1).Y);
    }

    [Theory]
    [InlineData("""{"attributes":{}}""", "SOURCE_ID_MISSING")]
    [InlineData("""{"attributes":{"OBJECTID":null}}""", "SOURCE_ID_MISSING")]
    [InlineData("""{"attributes":{"OBJECTID":"abc"}}""", "SOURCE_ID_INVALID")]
    [InlineData("""{"attributes":{"OBJECTID":1}}""", "GEOMETRY_PATHS_MISSING")]
    [InlineData("""{"attributes":{"OBJECTID":1},"geometry":{"paths":[]}}""", "GEOMETRY_PATHS_MISSING")]
    [InlineData("""{"attributes":{"OBJECTID":1},"geometry":{"paths":[[[1,2]]]}}""", "GEOMETRY_PATH_TOO_SHORT")]
    [InlineData("""{"attributes":{"OBJECTID":1},"geometry":{"paths":[[[1],[2,3]]]}}""", "GEOMETRY_COORDINATE_INVALID")]
    [InlineData("""{"attributes":{"OBJECTID":1},"geometry":{"paths":[[["x",2],[2,3]]]}}""", "GEOMETRY_COORDINATE_INVALID")]
    [InlineData("""{"attributes":{"OBJECTID":1},"geometry":{"paths":[[[1e400,2],[2,3]]]}}""", "GEOMETRY_COORDINATE_INVALID")]
    [InlineData("""{"attributes":{"OBJECTID":1},"geometry":{"paths":[[[1,2],[1,2]]]}}""", "GEOMETRY_POINTS_IDENTICAL")]
    public void InvalidRecordGoesToQuarantine(
        string feature, string expectedCode)
    {
        var result = Process("[" + feature + "]");

        Assert.Equal(1, result.Read);
        Assert.Empty(result.Assets);

        var error = Assert.Single(result.Quarantine);
        Assert.Equal(JobId, error.JobId);
        Assert.Equal(1, error.RecordNumber);
        Assert.Equal(expectedCode, error.ReasonCode);
    }

    [Fact]
    // Test die controleert of dubbele OBJECTID's correct worden afgewezen voordat de geometrie wordt gemapt.
    public void AllDuplicateIdsAreRejectedBeforeGeometryMapping()
    {
        var result = Process("""
        [
            {
                "attributes": { "OBJECTID": 7 },
                "geometry": { "paths": [[[1,2],[3,4]]] }
            },
            {
                "attributes": { "OBJECTID": 7 },
                "geometry": { "paths": [] }
            },
            {
                "attributes": { "OBJECTID": 8 },
                "geometry": { "paths": [[[5,6],[7,8]]] }
            }
        ]
        """);

        Assert.Equal(3, result.Read);
        Assert.Equal("8", Assert.Single(result.Assets).SourceRecordId);
        Assert.Equal(2, result.Quarantine.Count);

        Assert.All(result.Quarantine, error =>
        {
            Assert.Equal("7", error.SourceRecordId);
            Assert.Equal("SOURCE_ID_DUPLICATE", error.ReasonCode);
        });

        Assert.Equal(
            new[] { 1, 2 },
            result.Quarantine.Select(error => error.RecordNumber).ToArray());
    }

    [Fact]
    // Test die controleert of een record met meerdere paden correct wordt verwerkt en dat de mapping behouden blijft.
    public void OneInvalidPathRejectsTheWholeRecord()
    {
        var result = Process("""
        [
            {
                "attributes": { "OBJECTID": 9 },
                "geometry": {
                    "paths": [
                        [[1,2],[3,4]],
                        [[5,6]]
                    ]
                }
            }
        ]
        """);

        Assert.Empty(result.Assets);
        Assert.Equal(
            "GEOMETRY_PATH_TOO_SHORT",
            Assert.Single(result.Quarantine).ReasonCode);
    }

    [Fact]
    // Test die controleert of een lege selectie correct wordt verwerkt en dat de tellingen nul zijn.
    public void EmptySelectionHasZeroCounts()
    {
        var result = Process("[]");

        Assert.Equal(0, result.Read);
        Assert.Empty(result.Assets);
        Assert.Empty(result.Quarantine);
    }

    [Fact]
    // Test die controleert of de mapping geometrie behoudt buiten de selectie bbox.
    public void MappingPreservesGeometryOutsideTheSelectionBbox()
    {
        var result = Process("""
    [
        {
            "attributes": { "OBJECTID": 10 },
            "geometry": {
                "paths": [
                    [[210600,467900],[210900,467900]]
                ]
            }
        }
    ]
    """);

        var asset = Assert.Single(result.Assets);
        var line = Assert.IsType<LineString>(
            asset.Geometry.GetGeometryN(0));

        Assert.Empty(result.Quarantine);
        Assert.Equal(2, line.NumPoints);
        Assert.Equal(210600d, line.GetCoordinateN(0).X);
        Assert.Equal(210900d, line.GetCoordinateN(1).X);
        Assert.Equal(300d, line.Length);
    }

    private static ImportResult Process(string json)
    {
        using var document = JsonDocument.Parse(json);

        var result = LianderImportProcessor.Process(
            document.RootElement, JobId);

        Assert.Equal("liander-lv-v1", result.MappingVersion);

        Assert.Equal(
            result.Read,
            result.Assets.Count + result.Quarantine.Count);

        return result;
    }
}