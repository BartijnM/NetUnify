using NetTopologySuite.Geometries;

namespace NetUnify.Api.Models;

public sealed record Asset(
    Guid AssetId,
    string Operator,
    string SourceDataset,
    string SourceRecordId,
    string Discipline,
    string AssetType,
    string VoltageClass,
    double? VoltageKv,
    string? Material,
    MultiLineString Geometry,
    Guid JobId);