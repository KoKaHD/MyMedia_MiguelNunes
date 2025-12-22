using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMEDIA.Domain.DTOs
{
    public class ProdutoDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public decimal PrecoFinal { get; set; }
        public int Stock { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;

        public int FornecedorId { get; set; }
        public string FornecedorNome { get; set; } = string.Empty;

        public int CategoriaId { get; set; }
        public string CategoriaNome { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
    }
}
