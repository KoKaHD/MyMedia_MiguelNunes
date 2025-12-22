namespace MyMEDIA.API.DTOs
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public string TipoUtilizador { get; set; } = string.Empty;
    }
}
