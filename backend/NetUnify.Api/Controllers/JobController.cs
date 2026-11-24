using Microsoft.AspNetCore.Mvc;
using NetUnify.Api.Contracts;
using NetUnify.Api.Services;

namespace NetUnify.Api.Controllers;

[ApiController]
[Route("api/v1/jobs")]
public sealed class JobsController(JobService jobService) : ControllerBase
{
    [HttpGet]
    public ActionResult<JobsResponseDto> Get()
    {
        var response = new JobsResponseDto(
            jobService.GetJobs(),
            NextCursor: null);

        return Ok(response);
    }
}