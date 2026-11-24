namespace NetUnify.Api.Contracts;

public sealed record JobDto(
    Guid JobId,
    string Operator,
    string Dataset,
    string Status,
    DateTimeOffset SubmittedAt,
    JobCountsDto? Counts);

public sealed record JobCountsDto(
    long Read,
    long Accepted,
    long Quarantined);