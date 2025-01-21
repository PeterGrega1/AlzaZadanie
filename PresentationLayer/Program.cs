using ApplicationLayer.Config;
using DataLayer.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ApplicationLayer.Services;  // Import the SeedDatabaseService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddLogging(); // Logging service

// Register services for Application and Data layers
builder.Services.AddApplicationLayer();
builder.Services.AddDataLayer(builder.Configuration.GetConnectionString("DefaultConnection"));

// API versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Versioned API explorer for Swagger
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Add Swagger
builder.Services.AddSwaggerGen();

// Build the app
var app = builder.Build();

// Seed the database if needed (after the app is built)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedDatabaseService.SeedDatabase(services);  // Call the seed method
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
