namespace MyMEDIA.API.DTOs
{
    public class ProdutoCreateDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public int Stock { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
        public int CategoriaId { get; set; }
        public int FornecedorId { get; set; }
    }
}
