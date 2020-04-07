using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using RefactorThis.Domain;

namespace RefactorThis.Repository
{
    public class ProductOptionRepository : BaseRepository, IProductOptionRepository
    {
        public ProductOptionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IList<ProductOption>> SearchProductOptionsAsync(string productId = null)
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
                using (var rdr = await cmd.ExecuteReaderAsync())
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

        public async Task<ProductOption> SelectProductOptionAsync(Guid id)
        {
            using (var conn = NewConnection())
            {
                SqliteCommand cmd = new SqliteCommand($"select * from productoptions where id = '{id}' collate nocase",
                    conn);
                conn.Open();
                using (var rdr = await cmd.ExecuteReaderAsync())
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

        public async Task SaveProductOptionAsync(Guid id, Guid productId, string name, string description)
        {
            using (var conn = this.NewConnection())
            {
                var cmd = new SqliteCommand(
                        $"insert into productoptions (id, productid, name, description) values ('{id}', '{productId}', '{name}', '{description}')"
                    , conn);
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
            }
        }

        public async Task UpdateProductOptionAsync(Guid id, string name, string description)
        {
            using (var conn = this.NewConnection())
            {
                var cmd = new SqliteCommand(
                    $"update productoptions set name = '{name}', description = '{description}' where id = '{id}' collate nocase",
                    conn);
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
            }
        }

        public async Task DeleteProductOptionAsync(Guid id)
        {
            using (var conn = NewConnection())
            {
                conn.Open();
                var cmd = new SqliteCommand($"delete from productoptions where id = '{id}' collate nocase", conn);
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
            }
        }

    }
}
