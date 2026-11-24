using System.Text.Json;
using NetTopologySuite.Geometries;

namespace NetUnify.Api.Mapping;

public static class LianderGeometryMapper
{
    public static MultiLineString Map(JsonElement geometry)
    {
        if (geometry.ValueKind != JsonValueKind.Object
            || !geometry.TryGetProperty("paths", out var paths)
            || paths.ValueKind != JsonValueKind.Array
            || paths.GetArrayLength() == 0)
        {
            throw new FormatException("GEOMETRY_PATHS_MISSING");
        }

        var factory = NetTopologySuite.NtsGeometryServices.Instance
            .CreateGeometryFactory(srid: 28992);
        var lines = new List<LineString>();

        foreach (var path in paths.EnumerateArray())
        {
            if (path.ValueKind != JsonValueKind.Array
                || path.GetArrayLength() < 2)
            {
                throw new FormatException("GEOMETRY_PATH_TOO_SHORT");
            }

            var coordinates = new List<Coordinate>();

            foreach (var point in path.EnumerateArray())
            {
                if (point.ValueKind != JsonValueKind.Array
                    || point.GetArrayLength() < 2
                    || point[0].ValueKind != JsonValueKind.Number
                    || point[1].ValueKind != JsonValueKind.Number
                    || !point[0].TryGetDouble(out var x)
                    || !point[1].TryGetDouble(out var y)
                    || !double.IsFinite(x)
                    || !double.IsFinite(y))
                {
                    throw new FormatException("GEOMETRY_COORDINATE_INVALID");
                }

                coordinates.Add(new Coordinate(x, y));
            }

            var first = coordinates[0];

            if (!coordinates.Any(point => !point.Equals2D(first)))
            {
                throw new FormatException("GEOMETRY_POINTS_IDENTICAL");
            }

            lines.Add(factory.CreateLineString(coordinates.ToArray()));
        }

        return factory.CreateMultiLineString(lines.ToArray());
    }
}