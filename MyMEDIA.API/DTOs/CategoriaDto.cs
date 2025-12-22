namespace MyMEDIA.API.DTOs
{
    public class CategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int? CategoriaPaiId { get; set; }
    }
}