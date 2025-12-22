using MyMEDIA.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMEDIA.Domain.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public decimal PrecoFinal { get; set; }
        public int Stock { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;

        public int FornecedorId { get; set; }
        public Fornecedor Fornecedor { get; set; } = null!;

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } = null!;

        public EstadoProdutoEnum Estado { get; set; } = EstadoProdutoEnum.Pendente;
    }
}
