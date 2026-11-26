using NetTopologySuite.Geometries;
using NetUnify.Api.Mapping;
using Xunit;

namespace NetUnify.Api.Tests;

public sealed class RdToWgs84TransformerTests
{
    [Fact]
    // Test die controleert of de referentiepunten correct worden getransformeerd en dat de originele geometrie behouden blijft.
    public void ReferencePointMatchesAndOriginalGeometryIsPreserved()
    {
        var factory = NetTopologySuite.NtsGeometryServices.Instance
            .CreateGeometryFactory(srid: 28992);

        var original = factory.CreateMultiLineString(
        [
            factory.CreateLineString(
            [
                new Coordinate(210650, 467800),
                new Coordinate(210750, 467800)
            ])
        ]);

        var result = RdToWgs84Transformer.ToWgs84(original);

        Assert.NotSame(original, result);
        Assert.Equal(4326, result.SRID);
        Assert.Equal(1, result.NumGeometries);

        var line = Assert.IsType<LineString>(result.GetGeometryN(0));
        Assert.Equal(2, line.NumPoints);

        const double tolerance = 0.00001;
        var point = line.GetCoordinateN(0);

        Assert.InRange(
            point.X, 6.201141586 - tolerance, 6.201141586 + tolerance);
        Assert.InRange(
            point.Y, 52.195506749 - tolerance, 52.195506749 + tolerance);

        Assert.Equal(28992, original.SRID);

        var originalLine = Assert.IsType<LineString>(
            original.GetGeometryN(0));

        Assert.Equal(210650d, originalLine.GetCoordinateN(0).X);
        Assert.Equal(467800d, originalLine.GetCoordinateN(0).Y);
        Assert.Equal(210750d, originalLine.GetCoordinateN(1).X);
        Assert.Equal(467800d, originalLine.GetCoordinateN(1).Y);
    }

    [Fact]
    // Test die controleert of meerdere paden en de volgorde van punten behouden blijven na transformatie.
    public void TransformationPreservesMultiplePathsAndPointOrder()
    {
        var factory = NetTopologySuite.NtsGeometryServices.Instance
            .CreateGeometryFactory(srid: 28992);

        var original = factory.CreateMultiLineString(
        [
            factory.CreateLineString(
        [
            new Coordinate(210650, 467800),
            new Coordinate(210750, 467800),
            new Coordinate(210850, 467800)
        ]),
        factory.CreateLineString(
        [
            new Coordinate(210650, 467900),
            new Coordinate(210650, 468000)
        ])
        ]);

        var result = RdToWgs84Transformer.ToWgs84(original);

        Assert.Equal(4326, result.SRID);
        Assert.Equal(2, result.NumGeometries);

        var first = Assert.IsType<LineString>(result.GetGeometryN(0));
        var second = Assert.IsType<LineString>(result.GetGeometryN(1));

        Assert.Equal(3, first.NumPoints);
        Assert.Equal(2, second.NumPoints);
        Assert.Equal(4326, first.SRID);
        Assert.Equal(4326, second.SRID);

        // Het eerste pad loopt oostwaarts: lengtegraad neemt toe.
        Assert.True(first.GetCoordinateN(0).X < first.GetCoordinateN(1).X);
        Assert.True(first.GetCoordinateN(1).X < first.GetCoordinateN(2).X);

        // Het tweede pad loopt noordwaarts: breedtegraad neemt toe.
        Assert.True(second.GetCoordinateN(0).Y < second.GetCoordinateN(1).Y);
    }

    [Fact]
    // Test die controleert of een geometrie met een verkeerde SRID wordt afgewezen.
    public void GeometryWithWrongSridIsRejected()
    {
        var factory = NetTopologySuite.NtsGeometryServices.Instance
            .CreateGeometryFactory(srid: 4326);

        var geometry = factory.CreateMultiLineString(
        [
            factory.CreateLineString(
        [
            new Coordinate(6.201141586, 52.195506749),
            new Coordinate(6.202, 52.196)
        ])
        ]);

        var exception = Assert.Throws<ArgumentException>(
            () => RdToWgs84Transformer.ToWgs84(geometry));

        Assert.Equal("geometry", exception.ParamName);
        Assert.Equal(4326, geometry.SRID);
    }
}

