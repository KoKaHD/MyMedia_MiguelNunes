using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMEDIA.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; } = string.Empty;
        public string TipoUtilizador { get; set; } = "Cliente"; // Cliente, Fornecedor, Funcionario, Admin
        public string Estado { get; set; } = "Pendente"; // Pendente, Ativo
        public int? FornecedorId { get; set; }
    }

}
