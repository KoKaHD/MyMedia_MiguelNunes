using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMEDIA.Domain.Entities
{
    public class Encomenda
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public string UserId { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public bool Pago { get; set; } = false;
        public bool Expedido { get; set; } = false;
        public string Morada { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public ICollection<ItemEncomenda> Itens { get; set; } = new List<ItemEncomenda>();
    }
}
