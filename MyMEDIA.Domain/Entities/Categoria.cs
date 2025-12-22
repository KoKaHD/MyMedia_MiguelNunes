using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMEDIA.Domain.Entities
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public int? CategoriaPaiId { get; set; }
        public Categoria? CategoriaPai { get; set; }

        public ICollection<Categoria> SubCategorias { get; set; } = new List<Categoria>();
        public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
    }
}
