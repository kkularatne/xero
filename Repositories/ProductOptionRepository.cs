using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using RefactorThis.Models;

namespace RefactorThis.Repositories
{
    public interface IProductOptionRepository
    {
        IList<ProductOption> SearchProducts(string productId = null);
        ProductOption SelectProductOption(Guid id);
    }

    public class ProductOptionRepository : BaseRepository, IProductOptionRepository
    {
        public ProductOptionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IList<ProductOption> SearchProducts(string productId = null)
        {
            var items = new List<ProductOption>();
            var where = string.Empty;
            if (!string.IsNullOrEmpty(productId))
            {
                where = $"where productid = '{productId}' collate nocase";
            }

            using (var conn = NewConnection())
            {
                var cmd = new SqliteCommand($"select * from productoptions {where}", conn);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var productOption = new ProductOption()
                        {
                            Id = Guid.Parse(rdr["Id"].ToString()),
                            ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                            Name = rdr["Name"].ToString(),
                            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString()
                        };
                        items.Add(productOption);
                    }
                }

                conn.Close();
            }

            return items;
        }

        public ProductOption SelectProductOption(Guid id)
        {
            using (var conn = NewConnection())
            {
                SqliteCommand cmd = new SqliteCommand($"select * from productoptions where id = '{id}' collate nocase",
                    conn);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read())
                        throw new RecordNotFoundException(id, "product option");

                    return new ProductOption()
                    {
                        Id = Guid.Parse(rdr["Id"].ToString()),
                        ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                        Name = rdr["Name"].ToString(),
                        Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString()
                    };
                }
            }
        }
    }
}
