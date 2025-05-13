using Esfsg.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Esfsg.Application.Services
{
    public class PixService : IPixService
    {

        #region Construtor
        private readonly IHttpClientFactory _httpClient;
        private readonly IConfiguration _configuration;
        public PixService(IHttpClientFactory httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        #endregion

        public async Task GerarPixInscricao()
        {
            var result = await GerarPix(Convert.ToDecimal(10), "inscricao_001");
        }

        private async Task<string> GerarPOS()
        {
            var url = _configuration["PagamentoSettings:url_base"];
            var accessToken = _configuration["PagamentoSettings:access_token"];

            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var posRequest = new
            {
                external_store_id = "esfrsg",
                external_id = "001",
                category = 5812
            };

            var response = await client.PostAsJsonAsync($"{url}/pos", posRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao criar POS: {error}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(responseBody);
            return json.id;
        }

        private async Task<string?> GerarPix(decimal valor, string externalReference)
        {
            var url = _configuration["PagamentoSettings:url_base"];
            var accessToken = _configuration["PagamentoSettings:access_token"];

            var client = _httpClient.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var qrRequest = new
            {
                external_reference = externalReference,
                total_amount = valor
            };

            var deviceId = await GerarPOS();

            var response = await client.PostAsJsonAsync($"{url}/point/integration-api/devices/{deviceId}/qrs", qrRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao gerar QR Code: {error}");
            }

            return await response.Content.ReadAsStringAsync();
        }

    }
}
