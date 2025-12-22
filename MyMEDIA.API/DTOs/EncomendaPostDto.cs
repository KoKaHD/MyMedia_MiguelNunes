namespace MyMEDIA.API.DTOs
{
    public class EncomendaPostDto
    {
        public string Morada { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string MetodoPagamento { get; set; } = string.Empty;
        public List<ItemDto> Itens { get; set; } = new();
    }

    public class ItemDto
    {
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}