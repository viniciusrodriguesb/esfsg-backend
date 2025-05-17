using Esfsg.Application.DTOs.Request;
using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Filtros;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Esfsg.Application.Services
{
    public class CheckInService : ICheckInService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public CheckInService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<List<CheckInListaResponse>> Consultar(ConsultaCheckInRequest request)
        {
            var query = _context.CHECK_IN
                                 .AsNoTracking()
                                 .Include(i => i.IdInscricaoNavigation)
                                    .ThenInclude(u => u.IdUsuarioNavigation)
                                        .ThenInclude(c => c.IdClasseNavigation)
                                 .Include(i => i.IdInscricaoNavigation)
                                    .ThenInclude(f => f.IdFuncaoEventoNavigation)
                                 .AsQueryable();

            var result = await query.AplicarFiltro(request)
                                    .Select(x => new CheckInListaResponse()
                                    {
                                        IdPresenca = x.Id,
                                        Presenca = x.Presente,
                                        Nome = x.IdInscricaoNavigation.IdUsuarioNavigation.NomeCompleto,
                                        Evento = new DadosEventoResponse()
                                        {
                                            Periodo = x.IdInscricaoNavigation.Periodo,
                                            FuncaoEvento = x.IdInscricaoNavigation.IdFuncaoEventoNavigation.Descricao
                                        },
                                        Igreja = new DadosIgrejaResponse()
                                        {
                                            Igreja = x.IdInscricaoNavigation.IdUsuarioNavigation.IdIgrejaNavigation.Nome,
                                            Classe = x.IdInscricaoNavigation.IdUsuarioNavigation.IdClasseNavigation.Descricao
                                        }
                                    }).ToListAsync();

            return result;
        }


        public async Task ConfirmarPresenca(ValidaPresencaRequest request)
        {
            int Id = 0;

            if (request.IdCheckIn.HasValue)
                Id = request.IdCheckIn.Value;
            else if (!string.IsNullOrEmpty(request.QrCode))
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(request.QrCode));
                Id = int.Parse(decoded);
            }
            else
                throw new ArgumentException("Para validar é necessário o ID ou o QRCode.");

            var result = await _context.CHECK_IN.Where(x => x.Id == Id)
                                               .ExecuteUpdateAsync(s => s.SetProperty(p => p.Presente, request.Presenca));

            if (result == 0)
                throw new ArgumentException("Check-in não encontrado.");
        }

    }
}
