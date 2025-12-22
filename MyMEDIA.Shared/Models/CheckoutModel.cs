using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyMEDIA.Shared.Models
{
    public class CheckoutModel
    {
        [Required]
        public string Morada { get; set; } = string.Empty;

        [Required]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        public string CodigoPostal { get; set; } = string.Empty;

        [Required]
        public string MetodoPagamento { get; set; } = "Cartão"; // Cartão, MBWay, Referência
    }
}
