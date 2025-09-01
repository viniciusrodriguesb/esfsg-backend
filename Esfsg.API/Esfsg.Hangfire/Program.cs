using Esfsg.Hangfire.Configurations;
using Esfsg.Hangfire.Jobs;
using Esfsg.Infra.CrossCutting.IoC;
using Hangfire;
using Hangfire.Dashboard;

var builder = WebApplication.CreateBuilder(args);

#region Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
#endregion

#region Add Jobs
builder.Services.AddHangfireServices(builder.Configuration);

builder.Services.AddScoped<EmailQrCodeAcessoJob>();
builder.Services.AddScoped<EmailInscricaoConfirmadaJob>();
builder.Services.AddScoped<EmailQrCodePagamentoJob>();
builder.Services.AddScoped<EmailCancelamentoJob>();
builder.Services.AddScoped<EmailReembolsoJob>();
builder.Services.AddScoped<GerarPagamentoJob>();
builder.Services.AddScoped<AlteraStatusInscricaoPagamentoJob>();
#endregion

builder.Services.AddTransient<IDashboardAuthorizationFilter, HangfireAuthorization>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

JobsConfiguration.ConfigureJobs(app.Services);
app.UseHangfireDashboard(string.Empty, new DashboardOptions
{
    Authorization = new[] { app.Services.GetRequiredService<IDashboardAuthorizationFilter>() }
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
