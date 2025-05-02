using Esfsg.Infra.CrossCutting.IoC;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//Add Log
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//Add FluentValidation
builder.Services.AddValidators();

//Add Services
builder.Services.AddServices(builder.Configuration);

//Add Controllers
builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
