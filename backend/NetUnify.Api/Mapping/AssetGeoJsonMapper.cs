using NetUnify.Api.Models;

namespace NetUnify.Api.Mapping;

public static class AssetGeoJsonMapper
{
    public static object CreateFeatureCollection(
        IReadOnlyList<Asset> assets)
    {
        ArgumentNullException.ThrowIfNull(assets);

        return new
        {
            type = "FeatureCollection",
            features = assets.Select(asset =>
            {
                var projected =
                    RdToWgs84Transformer.ToWgs84(asset.Geometry);

                var paths = Enumerable.Range(0, projected.NumGeometries)
                    .Select(index => projected.GetGeometryN(index)
                        .Coordinates
                        .Select(point => new[] { point.X, point.Y })
                        .ToArray())
                    .ToArray();

                return new
                {
                    type = "Feature",
                    id = asset.AssetId.ToString(),
                    properties = new
                    {
                        asset.SourceRecordId,
                        asset.Operator,
                        asset.SourceDataset
                    },
                    geometry = new
                    {
                        type = "MultiLineString",
                        coordinates = paths
                    }
                };
            }).ToArray()
        };
    }
}