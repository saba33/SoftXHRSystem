using FluentValidation;
using FluentValidation.AspNetCore;
using HRSystem.API.Extensions;
using HRSystem.Infrastructure.Extensions;
using HRSystem.Infrastructure.Persistence;
using HRSystem.Application.Validation.Auth;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddSerilogLogging();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<HRDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnectionString")));

builder.Services.AddDependencies();

builder.Services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

app.UseGlobalExceptionHandler();
app.UseRequestLogging();

app.UseHttpsRedirection();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapControllers();

app.Run();