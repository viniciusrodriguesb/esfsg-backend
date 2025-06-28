using Esfsg.Application.Interfaces;
using Esfsg.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Esfsg.Infra.CrossCutting.IoC
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IMemoryCacheService, MemoryCacheService>();
            services.AddScoped<IClasseService, ClassesService>();
            services.AddScoped<IFuncoesService, FuncoesService>();
            services.AddScoped<IInstrumentoService, InstrumentoService>();
            services.AddScoped<ICondicaoMedicaService, CondicaoMedicaService>();
            services.AddScoped<IEventoService, EventoService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IInscricaoService, InscricaoService>();
            services.AddScoped<ICheckInService, CheckInService>();
            services.AddScoped<IQrCodeService, QRCodeService>();
            services.AddScoped<IEmailStatusService, EmailStatusService>();
            services.AddScoped<IPastorService, PastorService>();
            services.AddScoped<IRegiaoService, RegiaoService>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IGestaoInscricaoService, GestaoInscricaoService>();
            services.AddScoped<IGestaoPagamentoService, GestaoPagamentoService>();
            services.AddScoped<IVisitaService, VisitaService>();
            services.AddScoped<IIgrejaService, IgrejaService>();
            services.AddScoped<IDashboardService, DashboardService>();
        }
    }
}
