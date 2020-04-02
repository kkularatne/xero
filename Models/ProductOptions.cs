using System;
using System.Collections.Generic;

namespace RefactorThis.Models
{
    public interface IProductOptions
    {
        List<ProductOption> Items { get; }
    }

    public class ProductOptions : IProductOptions
    {
        private readonly IHelpers _helpers;

        public List<ProductOption> Items { get; private set; }

        public ProductOptions(IHelpers helpers)
        {
            _helpers = helpers;
            LoadProductOptions(null);
        }

        public ProductOptions(Guid productId, IHelpers helpers)
        {
            _helpers = helpers;
            LoadProductOptions($"where productid = '{productId}' collate nocase");
        }

        private void LoadProductOptions(string where)
        {
            Items = new List<ProductOption>();
            var conn = _helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"select id from productoptions {where}";

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr.GetString(0));
                Items.Add(new ProductOption(id,_helpers));
            }
        }
    }
}