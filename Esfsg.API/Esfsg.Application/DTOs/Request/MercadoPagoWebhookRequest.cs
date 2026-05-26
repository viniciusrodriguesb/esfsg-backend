using System.Text.Json.Serialization;

namespace Esfsg.Application.DTOs.Request
{
    public class MercadoPagoWebhookRequest
    {
        [JsonPropertyName("id")]
        public long? Id { get; set; }

        [JsonPropertyName("live_mode")]
        public bool LiveMode { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("date_created")]
        public DateTimeOffset? DateCreated { get; set; }

        [JsonPropertyName("user_id")]
        public long? UserId { get; set; }

        [JsonPropertyName("api_version")]
        public string? ApiVersion { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("data")]
        public MercadoPagoWebhookDataRequest? Data { get; set; }
    }

    public class MercadoPagoWebhookDataRequest
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
    }
}
