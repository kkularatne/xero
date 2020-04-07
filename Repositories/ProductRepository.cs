using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using RefactorThis.Models;

namespace RefactorThis.Repositories
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Product> SelectProductAsync(Guid id)
        {
            using (var conn = NewConnection())
            {
                SqliteCommand cmd = new SqliteCommand($"select * from Products where id = '{id}' collate nocase",conn);
                conn.Open();
                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    if (!rdr.Read())
                        throw new RecordNotFoundException(id, "product");

                    return new Product()
                    {
                        Id = Guid.Parse(rdr["Id"].ToString()),
                        Name = rdr["Name"].ToString(),
                        Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                        Price = decimal.Parse(rdr["Price"].ToString()),
                        DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
                    };
                }
            }
        }

        public async Task SaveProductAsync(Guid id, string name, string description, decimal price, decimal deliveryPrice)
        {
            using (var conn = this.NewConnection())
            {
                var cmd = new SqliteCommand(
                    $"insert into Products (id, name, description, price, deliveryprice) values ('{id}', '{name}', '{description}', {price}, {deliveryPrice})",
                    conn);
                conn.Open();
               await cmd.ExecuteNonQueryAsync();
                conn.Close();
            }
        }

        public async Task UpdateProductAsync(Guid id, string name, string description, decimal price, decimal deliveryPrice)
        {
            using (var conn = this.NewConnection())
            {
                var cmd = new SqliteCommand(
                    $"update Products set name = '{name}', description = '{description}', price = {price}, deliveryprice = {deliveryPrice} where id = '{id}' collate nocase",
                    conn);
                conn.Open();
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
            }
        }

        public async Task DeleteProductAsync(Guid id)
        {
            using (var conn = NewConnection())
            {
                conn.Open();
                var cmd = new SqliteCommand($"delete from Products where id = '{id}' collate nocase", conn);
                await cmd.ExecuteNonQueryAsync();
                conn.Close();
            }
        }

        public async Task<IList<Product>> SearchProductsAsync(string name = null)
        {
            var items = new List<Product>();
            var where = string.Empty;
            if (!string.IsNullOrEmpty(name))
            {
                where = $"where lower(name) like '%{name.ToLower()}%'";
            }

            using (var conn = NewConnection())
            {
                var cmd = new SqliteCommand($"select * from Products {where}",conn);
                conn.Open();
                using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    while (rdr.Read())
                    {
                        var product = new Product()
                        {
                            Id = Guid.Parse(rdr["Id"].ToString()),
                            Name = rdr["Name"].ToString(),
                            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                            Price = decimal.Parse(rdr["Price"].ToString()),
                            DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString())
                        };
                        items.Add(product);
                    }
                }

                conn.Close();
            }

            return items;
        }
    }
}