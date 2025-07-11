﻿using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Enums;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class InscricaoController : ControllerBase
    {

        #region Construtor
        private readonly IInscricaoService _inscricaoService;
        private readonly IStatusService _statusService;
        private readonly IMemoryCacheService _memoryCacheService;
        public InscricaoController(IInscricaoService inscricaoService,
                                IMemoryCacheService memoryCacheService,
                                IStatusService statusService)
        {
            _inscricaoService = inscricaoService;
            _memoryCacheService = memoryCacheService;
            _statusService = statusService;
        }
        #endregion        

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta da inscrição do usuário no evento.")]
        public async Task<IActionResult> ConsultarInscricao([FromQuery] InscricaoEventoResquest request)
        {
            try
            {
                var result = await _inscricaoService.ConsultarInscricao(request);

                if (result == null)
                    return NotFound("Nenhum registro encontrado.");

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Realização da inscrição no evento solicitado.")]
        public async Task<IActionResult> RealizarInscricao([FromBody] InscricaoRequest request)
        {
            try
            {
                var result = await _inscricaoService.RealizarInscricao(request);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("cancelar")]
        [SwaggerOperation(Summary = "Cancelamento da inscrição no evento solicitado.")]
        public async Task<IActionResult> CancelarInscricao(int Id)
        {
            try
            {
                await _statusService.AtualizarStatusInscricao(StatusEnum.CANCELADA, Id);
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
