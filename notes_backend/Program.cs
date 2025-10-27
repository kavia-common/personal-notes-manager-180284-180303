using NotesBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    // Keep original property casing in JSON if needed, default is camelCase
                });

builder.Services.AddEndpointsApiExplorer();

// NSwag OpenAPI config with metadata
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Notes API";
    settings.Version = "1.0.0";
    settings.Description = "Simple Notes REST API with CRUD operations.";
    settings.DocumentName = "v1";
    settings.PostProcess = document =>
    {
        document.Tags = new[]
        {
            new NSwag.OpenApiTag { Name = "Notes", Description = "Operations related to notes." }
        }.ToList();
    };
});

// Dependency Injection
builder.Services.AddSingleton<INoteService, NoteService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Use routing and controllers
app.UseRouting();

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
    config.DocumentPath = "/openapi.json";
});

// Map controllers
app.MapControllers();

// Health check endpoint
app.MapGet("/", () => new { message = "Healthy" })
   .WithTags("Health");

// Ensure app runs; Kestrel will use launchSettings.json for port 3001 in Development
app.Run();