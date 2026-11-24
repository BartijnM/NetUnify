var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<NetUnify.Api.Services.JobService>();

builder.Services.AddHttpClient<NetUnify.Api.Connectors.LianderConnector>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/dev/liander/sample", async (
        NetUnify.Api.Connectors.LianderConnector connector,
        CancellationToken cancellationToken) =>
    {
        var features = await connector.GetFeaturesAsync(cancellationToken);
        var jobId = Guid.Parse("00000000-0000-0000-0000-000000000010");

        var result = NetUnify.Api.Services.LianderImportProcessor.Process(
            features, jobId);

        return Results.Ok(new
        {
            result.JobId,
            result.MappingVersion,
            counts = new
            {
                read = result.Read,
                accepted = result.Assets.Count,
                quarantined = result.Quarantine.Count
            },
            countsBalance =
                result.Read == result.Assets.Count + result.Quarantine.Count,
            errors = result.Quarantine
        });
    });
}

app.Run();
