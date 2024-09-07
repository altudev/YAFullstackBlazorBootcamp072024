using ChatGPTClone.Application;
using ChatGPTClone.Application.Common.Interfaces;
using ChatGPTClone.Infrastructure;
using ChatGPTClone.WebApi;
using ChatGPTClone.WebApi.Filters;
using ChatGPTClone.WebApi.Services;
using Microsoft.Extensions.Options;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Warning()
    .CreateLogger();

try
{
    Log.Warning("Starting web application");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilog();

    // Add services to the container.

    // Controller'ları servis olarak ekle
    builder.Services.AddControllers(opt =>
    {
        // Global exception filtresini ekle
        opt.Filters.Add<GlobalExceptionFilter>();
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    builder.Services.AddApplication();

    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddWebApi(builder.Configuration, builder.Environment);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    var requestLocalizationOptions = app.Services
        .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

    app.UseRequestLocalization(requestLocalizationOptions);

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

Func<IServiceProvider, object> EnvironmentManager(string webRootPath)
{
    throw new NotImplementedException();
}