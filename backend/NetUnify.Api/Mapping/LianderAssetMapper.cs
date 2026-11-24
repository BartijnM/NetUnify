using System.Globalization;
using System.Text.Json;
using NetUnify.Api.Models;

namespace NetUnify.Api.Mapping;

public static class LianderAssetMapper
{
    public const string MappingVersion = "liander-lv-v1";
    
    public static string GetSourceRecordId(JsonElement feature)
    {
        if (feature.ValueKind != JsonValueKind.Object
            || !feature.TryGetProperty("attributes", out var attributes)
            || attributes.ValueKind != JsonValueKind.Object
            || !attributes.TryGetProperty("OBJECTID", out var sourceId)
            || sourceId.ValueKind == JsonValueKind.Null)
        {
            throw new FormatException("SOURCE_ID_MISSING");
        }

        if (sourceId.ValueKind != JsonValueKind.Number
            || !sourceId.TryGetInt64(out var objectId))
        {
            throw new FormatException("SOURCE_ID_INVALID");
        }

        return objectId.ToString(CultureInfo.InvariantCulture);
    }

    public static Asset Map(JsonElement feature, Guid jobId)
    {
        var sourceRecordId = GetSourceRecordId(feature);
        feature.TryGetProperty("geometry", out var sourceGeometry);
        var geometry = LianderGeometryMapper.Map(sourceGeometry);

        return new Asset(
            AssetId: Guid.NewGuid(),
            Operator: "Liander",
            SourceDataset: "Liander_Open_Data_Elektra/112",
            SourceRecordId: sourceRecordId,
            Discipline: "electricity",
            AssetType: "cable",
            VoltageClass: "LV",
            VoltageKv: null,
            Material: null,
            Geometry: geometry,
            JobId: jobId);
    }
}