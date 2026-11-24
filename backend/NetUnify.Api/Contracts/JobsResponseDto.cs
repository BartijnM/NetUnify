namespace NetUnify.Api.Contracts;

public sealed record JobsResponseDto(
    IReadOnlyList<JobDto> Items,
    string? NextCursor);
    