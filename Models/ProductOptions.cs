using System;
using System.Collections.Generic;
using RefactorThis.Repositories;

namespace RefactorThis.Models
{
    public interface IProductOptions
    {
        List<ProductOption> Items { get; }
    }

    public class ProductOptions : IProductOptions
    {
        private readonly IProductRepository _productRepository;

        public List<ProductOption> Items { get; private set; }

        public ProductOptions(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            LoadProductOptions($"where productid = '{productId}' collate nocase");
        }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();
            var conn = _productRepository.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"select id from productoptions {where}";

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr.GetString(0));
                Items.Add(new ProductOption(id,_productRepository));
            }
        }
    }
}