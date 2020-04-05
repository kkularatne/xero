using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using RefactorThis.Models;

namespace RefactorThis.Repositories
{
    public class ProductOptionRepository : BaseRepository, IProductOptionRepository
    {
        public ProductOptionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IList<ProductOption> SearchProductOptions(string productId = null)
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

        public void SaveProductOption(Guid id, Guid productId, string name, string description)
        {
            using (var conn = this.NewConnection())
            {
                var cmd = new SqliteCommand(
                        $"insert into productoptions (id, productid, name, description) values ('{id}', '{productId}', '{name}', '{description}')"
                    , conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void UpdateProductOption(Guid id, string name, string description)
        {
            using (var conn = this.NewConnection())
            {
                var cmd = new SqliteCommand(
                    $"update productoptions set name = '{name}', description = '{description}' where id = '{id}' collate nocase",
                    conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void DeleteProductOption(Guid id)
        {
            using (var conn = NewConnection())
            {
                conn.Open();
                var cmd = new SqliteCommand($"delete from productoptions where id = '{id}' collate nocase", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

    }
}
