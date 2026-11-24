using System.Text.Json;
using NetUnify.Api.Mapping;
using NetUnify.Api.Models;

namespace NetUnify.Api.Services;

public static class LianderImportProcessor
{
    public static ImportResult Process(JsonElement features, Guid jobId)
    {
        var assets = new List<Asset>();
        var quarantine = new List<QuarantineRecord>();
        var candidates = new List<(
            int RecordNumber,
            string SourceRecordId,
            JsonElement Feature)>();

        var recordNumber = 0;

        foreach (var feature in features.EnumerateArray())
        {
            recordNumber++;

            try
            {
                var sourceId = LianderAssetMapper.GetSourceRecordId(feature);
                candidates.Add((recordNumber, sourceId, feature));
            }
            catch (FormatException exception)
            {
                quarantine.Add(new QuarantineRecord(
                    jobId, recordNumber, null, exception.Message));
            }
        }

        var duplicateIds = candidates
            .GroupBy(candidate => candidate.SourceRecordId)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToHashSet();

        foreach (var candidate in candidates)
        {
            if (duplicateIds.Contains(candidate.SourceRecordId))
            {
                quarantine.Add(new QuarantineRecord(
                    jobId,
                    candidate.RecordNumber,
                    candidate.SourceRecordId,
                    "SOURCE_ID_DUPLICATE"));

                continue;
            }

            try
            {
                assets.Add(LianderAssetMapper.Map(candidate.Feature, jobId));
            }
            catch (FormatException exception)
            {
                quarantine.Add(new QuarantineRecord(
                    jobId,
                    candidate.RecordNumber,
                    candidate.SourceRecordId,
                    exception.Message));
            }
        }

        return new ImportResult(
            jobId,
            MappingVersion: LianderAssetMapper.MappingVersion,
            Read: recordNumber,
            Assets: assets.ToArray(),
            Quarantine: quarantine
                .OrderBy(record => record.RecordNumber)
                .ToArray());
    }
}