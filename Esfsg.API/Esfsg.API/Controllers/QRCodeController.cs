using Esfsg.Application.DTOs.Response;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class QRCodeController : ControllerBase
    {

        #region Construtor
        private readonly IQrCodeService _qrCodeService;
        private readonly IMemoryCacheService _memoryCacheService;
        public QRCodeController(IQrCodeService qrCodeService,
                                IMemoryCacheService memoryCacheService)
        {
            _qrCodeService = qrCodeService;
            _memoryCacheService = memoryCacheService;
        }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GerarQRCodeAcesso([FromQuery] int IdInscricao)
        {
            string key = $"qr-code-{IdInscricao}";

            try
            {
                var response = _memoryCacheService.Get<QRCodeResponse>(key);

                if (response is null)
                {
                    response = await _qrCodeService.GerarQRCodeAcesso(IdInscricao);

                    if (response == null)
                        return NotFound("Nenhum registro encontrado.");

                    _memoryCacheService.Set(key, response, TimeSpan.FromMinutes(30));
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
