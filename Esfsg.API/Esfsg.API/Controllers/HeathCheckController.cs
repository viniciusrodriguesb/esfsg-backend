using Esfsg.Application.Interfaces;
using Esfsg.Infra.Data;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class HeathCheckController : ControllerBase
    {

        #region Constructor
        private readonly DbContextBase _dbContext;
        private readonly IEmailService _emailService;
        public HeathCheckController(DbContextBase dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }
        #endregion

        [HttpGet("database")]
        [SwaggerOperation(Summary = "Verifica a conexão com o banco de dados")]
        public async Task<IActionResult> CheckDatabase()
        {
            try
            {
                var canConnect = await _dbContext.Database.CanConnectAsync();

                return canConnect ? Ok() : StatusCode(500, "Erro ao conectar ao banco de dados.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("email")]
        [SwaggerOperation(Summary = "Verifica a conexão com o servidor de email")]
        public async Task<IActionResult> CheckEmail()
        {
            try
            {
                await _emailService.SendEmailAsync("vini240801@gmail.com", "teste subtitulo", "teste corpo");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
