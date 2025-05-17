using QRCoder;

namespace Esfsg.Application.Helpers
{
    public static class QRCodeHelper
    {
        public static string GenerateQrCodeBase64<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var plainText = value.ToString();

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new Base64QRCode(qrCodeData);

            return qrCode.GetGraphic(20); 
        }
    }
}
