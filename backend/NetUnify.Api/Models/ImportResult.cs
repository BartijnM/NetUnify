namespace NetUnify.Api.Models;

public sealed record QuarantineRecord(
    Guid JobId,
    int RecordNumber,
    string? SourceRecordId,
    string ReasonCode);

public sealed record ImportResult(
    Guid JobId,
    string MappingVersion,
    int Read,
    IReadOnlyList<Asset> Assets,
    IReadOnlyList<QuarantineRecord> Quarantine);