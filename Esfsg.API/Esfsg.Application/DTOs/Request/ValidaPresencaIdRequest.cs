namespace Esfsg.Application.DTOs.Request
{
    public class ValidaPresencaIdRequest
    {
        public List<int> Ids { get; set; } = new List<int>();
        public bool Presenca {  get; set; }
    }
}
