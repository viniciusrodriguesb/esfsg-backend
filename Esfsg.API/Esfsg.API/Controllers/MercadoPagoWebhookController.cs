using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/webhooks/mercadopago")]
    public class MercadoPagoWebhookController : ControllerBase
    {

        #region Construtor
        private readonly IPagamentoService _pagamentoService;
        private readonly ILogger<MercadoPagoWebhookController> _logger;
        public MercadoPagoWebhookController(IPagamentoService pagamentoService,
                                            ILogger<MercadoPagoWebhookController> logger)
        {
            _pagamentoService = pagamentoService;
            _logger = logger;
        } 
        #endregion

        [HttpPost]
        public async Task<IActionResult> ReceberWebhook([FromBody] MercadoPagoWebhookRequest request)
        {
            _logger.LogInformation(
                "Webhook Mercado Pago recebido. Type: {Type}, Action: {Action}, DataId: {DataId}",
                request.Type,
                request.Action,
                request.Data?.Id);

            if (!string.Equals(request.Type, "payment", StringComparison.OrdinalIgnoreCase))
                return Ok();

            if (string.IsNullOrWhiteSpace(request.Data?.Id))
                return Ok();

            await _pagamentoService.AtualizarPagamentoPorWebhook(request.Data.Id);

            return Ok();
        }
    }
}
