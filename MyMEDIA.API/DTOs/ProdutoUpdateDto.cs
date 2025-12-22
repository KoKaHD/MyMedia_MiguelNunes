using MyMEDIA.Domain.Enums;

namespace MyMEDIA.API.DTOs
{
    public class ProdutoUpdateDto : ProdutoCreateDto
    {
        public EstadoProdutoEnum Estado { get; set; }
    }
}