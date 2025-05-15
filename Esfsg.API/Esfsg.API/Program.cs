using Esfsg.Infra.CrossCutting.IoC;

var builder = WebApplication.CreateBuilder(args);

//Add Log
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

//Add Services
builder.Services.AddServices(builder.Configuration);

//Add Controllers
builder.Services.AddControllers();

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
