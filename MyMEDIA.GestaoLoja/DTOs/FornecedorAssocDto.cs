namespace MyMEDIA.GestaoLoja.DTOs
{
    public class FornecedorAssocDto
    {
        public string UserId { get; set; } = string.Empty;
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? FornecedorId { get; set; }
        public string FornecedorNome { get; set; } = string.Empty;
    }
}