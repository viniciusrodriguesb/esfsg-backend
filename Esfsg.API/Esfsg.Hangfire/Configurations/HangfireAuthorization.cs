using Esfsg.Application.DTOs;
using Hangfire.Dashboard;
using Microsoft.Extensions.Options;
using System.Text;

namespace Esfsg.Hangfire.Configurations
{
    public class HangfireAuthorization : IDashboardAuthorizationFilter
    {

        #region Construtor
        private readonly string _username;
        private readonly string _password;
        public HangfireAuthorization(IOptions<HangfireConfiguration> options)
        {
            _username = options.Value.Usuario;
            _password = options.Value.Senha;
        } 
        #endregion

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var header = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(header))
            {
                Challenge(httpContext);
                return false;
            }

            var authHeader = header.ToString();
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                Challenge(httpContext);
                return false;
            }

            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            string decodedCredentials;
            try
            {
                var credentialBytes = Convert.FromBase64String(encodedCredentials);
                decodedCredentials = Encoding.UTF8.GetString(credentialBytes);
            }
            catch
            {
                Challenge(httpContext);
                return false;
            }

            // Separa username e password
            var parts = decodedCredentials.Split(':');
            if (parts.Length != 2)
            {
                Challenge(httpContext);
                return false;
            }

            var username = parts[0];
            var password = parts[1];

            // Compara com os valores configurados
            if (username == _username && password == _password)
            {
                return true; // autorizado
            }

            Challenge(httpContext);
            return false;
        }

        private void Challenge(HttpContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
        }
    }
}
