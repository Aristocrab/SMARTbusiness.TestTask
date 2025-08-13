using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using SMARTbusiness.TestTask.Application.Database;
using SMARTbusiness.TestTask.Application.Services;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(optionsBuilder => 
    optionsBuilder.UseSqlServer(builder.Configuration["SqlServerConnectionString"]));

builder.Services.AddScoped<DbSeeder>();

builder.Services.AddScoped<IContractsService, ContractsService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await dbSeeder.SeedDb();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();