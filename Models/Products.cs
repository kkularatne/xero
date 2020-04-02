using System;
using System.Collections.Generic;

namespace RefactorThis.Models
{
    public interface IProducts
    {
        List<Product> Items { get; }
    }

    public class Products : IProducts
    {
        private readonly IHelpers _helpers;
        public List<Product> Items { get; private set; }

        public Products(IHelpers helpers)
        {
            this._helpers = helpers;
            LoadProducts(null);
        }

        public Products(string name, IHelpers helpers)
        {
            this._helpers = helpers;
            LoadProducts($"where lower(name) like '%{name.ToLower()}%'");
        }

        private void LoadProducts(string where)
        {
            Items = new List<Product>();
            var conn = _helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"select id from Products {where}";

            var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var id = Guid.Parse(rdr.GetString(0));
                Items.Add(new Product(id,_helpers));
            }
        }
    }
}