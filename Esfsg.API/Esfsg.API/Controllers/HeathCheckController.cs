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
        public HeathCheckController(DbContextBase dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        [HttpGet]
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
    }
}
