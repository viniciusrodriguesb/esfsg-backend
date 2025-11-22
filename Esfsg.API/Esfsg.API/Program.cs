using Esfsg.API.Middlewares;
using Esfsg.Infra.CrossCutting.IoC;

var builder = WebApplication.CreateBuilder(args);

#region Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
#endregion

builder.Services.AddServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseMiddleware<ValidationExceptionMiddleware>();

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