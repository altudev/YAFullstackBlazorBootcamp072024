using Microsoft.EntityFrameworkCore;
using PasswordStorageApp.WebApi.Hubs;
using PasswordStorageApp.WebApi.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddSignalR();

// Add Cors for all origins
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  
}

var cts = new CancellationTokenSource();

await using var scope = app.Services.CreateAsyncScope();

var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

var migrations = await context.Database.GetPendingMigrationsAsync(cts.Token);

if (migrations.Any())
    await context.Database.MigrateAsync(cts.Token);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<AccountsHub>("/hubs/accountsHub");

app.Run();
