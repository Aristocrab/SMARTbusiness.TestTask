using Hangfire;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using SMARTbusiness.TestTask.Application.Database;
using SMARTbusiness.TestTask.Application.Services;
using SMARTbusiness.TestTask.WebApi.Middleware;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints.",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-Api-Key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = Microsoft.OpenApi.Models.ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(optionsBuilder => 
    optionsBuilder.UseSqlServer(builder.Configuration["SqlServerConnectionString"]));

builder.Services.AddScoped<DbSeeder>();

builder.Services.AddTransient<ApiKeySecurityMiddleware>();

builder.Services.AddScoped<IContractsService, ContractsService>();

builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration["SqlServerConnectionString"]));
builder.Services.AddHangfireServer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await dbSeeder.SeedDb();
}

app.MapGet("/", httpContext =>
{
    // redirect to Swagger UI
    httpContext.Response.Redirect("/swagger/index.html");
    return Task.CompletedTask;
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
{
    appBuilder.UseMiddleware<ApiKeySecurityMiddleware>();
});
app.MapControllers();

app.Run();