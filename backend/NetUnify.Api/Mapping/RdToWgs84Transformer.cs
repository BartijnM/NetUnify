using NetTopologySuite.Geometries;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;

namespace NetUnify.Api.Mapping;

public static class RdToWgs84Transformer
{
    private const string RdDefinition = """
        PROJCS["Amersfoort / RD New",
            GEOGCS["Amersfoort",
                DATUM["Amersfoort",
                    SPHEROID["Bessel 1841",6377397.155,299.1528128],
                    TOWGS84[
                        565.4171,50.3319,465.5524,
                        -0.39895739,0.34398782,-1.87740164,4.0725
                    ]
                ],
                PRIMEM["Greenwich",0],
                UNIT["degree",0.0174532925199433]
            ],
            PROJECTION["Oblique_Stereographic"],
            PARAMETER["latitude_of_origin",52.1561605555556],
            PARAMETER["central_meridian",5.38763888888889],
            PARAMETER["scale_factor",0.9999079],
            PARAMETER["false_easting",155000],
            PARAMETER["false_northing",463000],
            UNIT["metre",1]
        ]
        """;

    public static MultiLineString ToWgs84(MultiLineString geometry)
    {
        ArgumentNullException.ThrowIfNull(geometry);

        if (geometry.SRID != 28992)
        {
            throw new ArgumentException(
                "De invoergeometrie moet RD New (SRID 28992) gebruiken.",
                nameof(geometry));
        }

        var coordinateSystemFactory = new CoordinateSystemFactory();
        var rd = coordinateSystemFactory.CreateFromWkt(RdDefinition);

        var transformation = new CoordinateTransformationFactory()
            .CreateFromCoordinateSystems(
                rd, GeographicCoordinateSystem.WGS84)
            .MathTransform;

        var factory = NetTopologySuite.NtsGeometryServices.Instance
            .CreateGeometryFactory(srid: 4326);

        var lines = new LineString[geometry.NumGeometries];

        for (var i = 0; i < geometry.NumGeometries; i++)
        {
            var sourceLine = (LineString)geometry.GetGeometryN(i);
            var coordinates = new Coordinate[sourceLine.NumPoints];

            for (var j = 0; j < sourceLine.NumPoints; j++)
            {
                var source = sourceLine.GetCoordinateN(j);

                if (!double.IsFinite(source.X)
                    || !double.IsFinite(source.Y))
                {
                    throw new ArgumentException(
                        "De invoer bevat een niet-eindige coördinaat.",
                        nameof(geometry));
                }

                var target = transformation.Transform(
                    new[] { source.X, source.Y });

                if (!double.IsFinite(target[0])
                    || !double.IsFinite(target[1]))
                {
                    throw new InvalidOperationException(
                        "De transformatie levert geen eindige coördinaat op.");
                }

                // X = lengtegraad, Y = breedtegraad.
                coordinates[j] = new Coordinate(target[0], target[1]);
            }

            lines[i] = factory.CreateLineString(coordinates);
        }

        return factory.CreateMultiLineString(lines);
    }
}