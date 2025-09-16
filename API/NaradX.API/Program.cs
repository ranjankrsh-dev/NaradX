using AutoMapper;
using NaradX.API.Extensions;
using NaradX.Business.Mappings;
using NaradX.Infrastructure.Data.Seed;

var builder = WebApplication.CreateBuilder(args);
var configuration=builder.Configuration;

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ContactProfile>();
    cfg.AddProfile<PaginatedListProfile>();
}, typeof(Program).Assembly);


// ========= SERVICE REGISTRATION =========
builder.Services.AddApiServices();                    // API-related services
builder.Services.AddThirdPartyServices();             // CORS, Health Checks, Auth
builder.Services.AddInfrastructureServices(configuration); // DbContext, External services
builder.Services.AddRepositoryServices();             // All repositories
builder.Services.AddApplicationServices();            // Business logic services

var app = builder.Build();

// Run database seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSecurityHeaders(new HeaderPolicyCollection()
    .AddContentTypeOptionsNoSniff() // Only this is really useful for APIs
                                    // Optional: Add these if your API serves to browsers
    .AddFrameOptionsDeny() // But usually APIs don't need this
);

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
