namespace Esfsg.Application.Helpers
{
    public static class FormatoHelper
    {
        public static string FormatarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return string.Empty;

            var numeros = new string(cpf.Where(char.IsDigit).ToArray());

            if (numeros.Length != 11)
                return cpf; 

            return Convert.ToUInt64(numeros).ToString(@"000\.000\.000\-00");
        }

        public static string FormatarTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return string.Empty;

            var numeros = new string(telefone.Where(char.IsDigit).ToArray());

            return numeros.Length switch
            {
                10 => Convert.ToUInt64(numeros).ToString(@"\(00\) 0000\-0000"),
                11 => Convert.ToUInt64(numeros).ToString(@"\(00\) 00000\-0000"),
                _ => telefone 
            };
        }
    }
}
