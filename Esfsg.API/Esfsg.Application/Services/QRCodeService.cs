using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Helpers;
using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Esfsg.Application.Services
{
    public class QRCodeService : IQrCodeService
    {

        #region Construtor
        private readonly DbContextBase _context;
        public QRCodeService(DbContextBase context)
        {
            _context = context;
        }
        #endregion

        public async Task<QRCodeResponse> GerarQRCodeAcesso(int IdInscricao)
        {
            var checkin = await _context.CHECK_IN
                                        .AsNoTracking()
                                        .Where(x => x.IdInscricao == IdInscricao)
                                        .FirstOrDefaultAsync();

            if (checkin is null)
                throw new ArgumentException("Inscrição no check-in não encontrada.");

            return new QRCodeResponse()
            {
                IdCheckIn = checkin.Id,
                ConteudoQrCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(checkin.Id.ToString())),
                ImagemBase64 = QRCodeHelper.GenerateQrCodeBase64(checkin.Id)
            };
        }


    }
}
