using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebAPI.Entities;

var policyName = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName,
                      builder =>
                      {
                          builder
                            .WithOrigins("http://localhost:4200")
                            .AllowAnyOrigin()
                            .AllowAnyHeader();
                      });
});

builder.Services.AddDbContext<WebAPIDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeAppCon"));
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policyName);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(
                    Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
