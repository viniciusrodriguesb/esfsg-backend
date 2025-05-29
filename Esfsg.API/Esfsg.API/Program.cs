using Esfsg.API.Hangfire.Configurations;
using Esfsg.API.Hangfire.Jobs;
using Esfsg.Infra.CrossCutting.IoC;
using Hangfire;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Add logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        #endregion

        #region Add Services
        builder.Services.AddServices(builder.Configuration);
        builder.Services.AddScoped<EmailQrCodeAcessoJob>();
        builder.Services.AddScoped<EmailInscricaoConfirmadaJob>();
        #endregion

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

        JobsConfiguration.ConfigureJobs(app.Services);
        app.UseHangfireDashboard("/jobs");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}

#region Add logging

#endregion
#region Add Services

#endregion
