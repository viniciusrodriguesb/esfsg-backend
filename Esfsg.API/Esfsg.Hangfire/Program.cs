using Esfsg.Hangfire.Configurations;
using Esfsg.Hangfire.Jobs;
using Esfsg.Infra.CrossCutting.IoC;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddServices(builder.Configuration);
builder.Services.AddScoped<ExampleJob>();

builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowAll");
app.UseHttpsRedirection();

JobsConfiguration.ConfigureJobs(app.Services);

app.UseHangfireDashboard("/hangfire");
app.Run();