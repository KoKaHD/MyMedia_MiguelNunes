using System.Collections.Generic;
using System.Linq;
using MyMEDIA.Shared.Models;
using MyMEDIA.Domain.DTOs;

namespace MyMEDIA.Shared.Services
{
    public class CarrinhoState
    {
        private readonly List<CarrinhoItem> _itens = new();

        public IReadOnlyList<CarrinhoItem> Itens => _itens.AsReadOnly();
        public int Quantidade => _itens.Sum(i => i.Quantidade);
        public decimal Total => _itens.Sum(i => i.Preco * i.Quantidade);

        public void Adicionar(ProdutoDTO produto)
        {
            var item = _itens.FirstOrDefault(i => i.ProdutoId == produto.Id);
            if (item == null)
            {
                _itens.Add(new CarrinhoItem
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.PrecoFinal,
                    Quantidade = 1,
                    ImagemUrl = produto.ImagemUrl
                });
            }
            else
            {
                item.Quantidade++;
            }
            NotifyChanged();
        }

        public void Remover(int produtoId)
        {
            var item = _itens.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (item != null)
            {
                _itens.Remove(item);
                NotifyChanged();
            }
        }

        public void Limpar()
        {
            _itens.Clear();
            NotifyChanged();
        }

        public event Action? OnChange;
        private void NotifyChanged() => OnChange?.Invoke();
    }
}
