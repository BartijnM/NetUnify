using NetUnify.Api.Contracts;

namespace NetUnify.Api.Services;

public sealed class JobService
{
    public IReadOnlyList<JobDto> GetJobs()
    {
        JobDto[] jobs =
        [
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000001"),
                "Liander",
                "Liander_Open_Data_Elektra/112",
                "succeeded",
                new DateTimeOffset(2026, 11, 19, 9, 0, 0, TimeSpan.Zero),
                new JobCountsDto(23, 21, 2)),
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000003"),
                "Liander",
                "Liander_Open_Data_Elektra/112",
                "queued",
                new DateTimeOffset(2026, 11, 19, 10, 0, 0, TimeSpan.Zero),
                null),
            new(
                Guid.Parse("00000000-0000-0000-0000-000000000002"),
                "Liander",
                "Liander_Open_Data_Elektra/112",
                "succeeded",
                new DateTimeOffset(2026, 11, 19, 10, 0, 0, TimeSpan.Zero),
                new JobCountsDto(0, 0, 0))
        ];

        return jobs
            .OrderByDescending(job => job.SubmittedAt)
            .ThenBy(job => job.JobId)
            .ToArray();
    }
}