﻿using Esfsg.Application.DTOs.Request;
using Esfsg.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Esfsg.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class EventoController : ControllerBase
    {
        #region Construtor
        private readonly IEventoService _eventoService;
        private readonly IMemoryCacheService _memoryCacheService;
        public EventoController(IEventoService eventoService,
                                IMemoryCacheService memoryCacheService)
        {
            _eventoService = eventoService;
            _memoryCacheService = memoryCacheService;
        }
        #endregion

        [HttpGet]
        [SwaggerOperation(Summary = "Consulta todos os eventos de uma região.")]
        public async Task<IActionResult> ConsultarEventos([FromQuery] int RegiaoId)
        {
            try
            {
                var result = await _eventoService.ConsultarEvento(RegiaoId);

                if (result == null || !result.Any())
                    return NotFound("Nenhum registro encontrado.");

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("periodos")]
        [SwaggerOperation(Summary = "Consulta todos os períodos disponíveis.")]
        public async Task<IActionResult> ConsultarPeriodos([FromQuery] int IdEvento)
        {
            try
            {
                var result = await _eventoService.ConsultarPeriodos(IdEvento);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Inclui um novo evento no banco de dados.")]
        public async Task<IActionResult> IncluirEvento([FromBody] EventoRequest request)
        {
            try
            {
                await _eventoService.IncluirEvento(request);
                return StatusCode(StatusCodes.Status201Created);
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

        [HttpDelete]
        [SwaggerOperation(Summary = "Exclui um evento no banco de dados.")]
        public async Task<IActionResult> ExcluirEvento([FromQuery] int Id)
        {
            try
            {
                await _eventoService.ExcluirEvento(Id);
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

        [HttpPut]
        [SwaggerOperation(Summary = "Edita um evento no banco de dados.")]
        public async Task<IActionResult> EditarEvento([FromQuery] int Id, [FromBody] AlteraEventoRequest request)
        {
            try
            {
                await _eventoService.EditarEvento(Id, request);
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
