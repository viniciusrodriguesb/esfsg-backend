using Esfsg.Application;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace Esfsg.API.Middlewares
{
    public class ValidationExceptionMiddleware
    {

        #region Construtor
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionMiddleware> _logger;
        public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        #endregion

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro capturado no middleware global: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            HttpStatusCode statusCode;
            string message = ex.Message;
            object? details = null;

            switch (ex)
            {
                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    details = validationEx.Errors;
                    break;

                case BusinessException:
                    statusCode = HttpStatusCode.BadRequest;
                    break;

                case ConflictException:
                    statusCode = HttpStatusCode.Conflict;
                    break;

                case NotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    break;

                case UnauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    break;

                case ForbiddenException:
                    statusCode = HttpStatusCode.Forbidden;
                    break;

                case DbUpdateConcurrencyException:
                    statusCode = HttpStatusCode.Conflict;
                    message = "Conflito de concorrência ao atualizar dados.";
                    break;

                case DbUpdateException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Erro ao salvar dados no banco de dados.";
                    break;

                case TimeoutException:
                    statusCode = HttpStatusCode.RequestTimeout;
                    message = "Tempo de requisição expirado.";
                    break;

                case HttpRequestException:
                    statusCode = HttpStatusCode.BadGateway;
                    message = "Falha ao se comunicar com serviço externo.";
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Ocorreu um erro inesperado no servidor.";
                    break;
            }

            response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                statusCode = response.StatusCode,
                message,
                details
            });

            await response.WriteAsync(result);
        }
    }
}
