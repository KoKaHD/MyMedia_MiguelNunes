namespace MyMEDIA.GestaoLoja.DTOs
{
    public class StatsDto
    {
        public int TotalVendas { get; set; }
        public decimal TotalFaturacao { get; set; }
        public int ProdutosAtivos { get; set; }
        public int ClientesAtivos { get; set; }
        public List<MesVendaDto> VendasMes { get; set; } = new();
        public List<ProdutoTopDto> Top5 { get; set; } = new();
    }

    public class MesVendaDto
    {
        public string Mes { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal Faturacao { get; set; }
    }

    public class ProdutoTopDto
    {
        public string Nome { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}