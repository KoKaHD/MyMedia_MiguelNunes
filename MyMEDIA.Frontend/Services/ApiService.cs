using System.Net.Http.Json;
using MyMEDIA.Domain.DTOs;
using MyMEDIA.API.DTOs;

namespace MyMEDIA.Frontend.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        public ApiService(HttpClient http) => _http = http;

        public async Task<List<ProdutoDTO>?> GetProdutosAsync(int? categoriaId, string? nome)
        {
            var url = $"api/produtos?tamanho=50";
            if (categoriaId.HasValue) url += $"&categoriaId={categoriaId}";
            if (!string.IsNullOrWhiteSpace(nome)) url += $"&nome={Uri.EscapeDataString(nome)}";

            var res = await _http.GetFromJsonAsync<ProdutoListResponse>(url);
            return res?.Items;
        }

        public async Task<ProdutoDTO?> GetProdutoAsync(int id) =>
            await _http.GetFromJsonAsync<ProdutoDTO>($"api/produtos/{id}");

        public async Task<List<CategoriaDto>?> GetCategoriasAsync() =>
            await _http.GetFromJsonAsync<List<CategoriaDto>>("api/categorias");

        public async Task<bool> PostEncomendaAsync(object dto) =>
            (await _http.PostAsJsonAsync("api/encomendas", dto)).IsSuccessStatusCode;
    }

    public record ProdutoListResponse(int Total, int Pagina, int Tamanho, List<ProdutoDTO> Items);
}