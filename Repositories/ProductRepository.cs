using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using RefactorThis.Models;

namespace RefactorThis.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Product SelectProduct(Guid id)
        {
            using (var conn = NewConnection())
            {
                SqliteCommand cmd = new SqliteCommand($"select * from Products where id = '{id}' collate nocase",conn);
                conn.Open();
                using (var rdr = cmd.ExecuteReader())
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

        public void SaveProduct(Guid id, string name, string description, decimal price, decimal deliveryPrice)
        {
            using (var conn = this.NewConnection())
            {
                var cmd = new SqliteCommand(
                    $"insert into Products (id, name, description, price, deliveryprice) values ('{id}', '{name}', '{description}', {price}, {deliveryPrice})",
                    conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void UpdateProduct(Guid id, string name, string description, decimal price, decimal deliveryPrice)
        {
            using (var conn = this.NewConnection())
            {
                var cmd = new SqliteCommand(
                    $"update Products set name = '{name}', description = '{description}', price = {price}, deliveryprice = {deliveryPrice} where id = '{id}' collate nocase",
                    conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void DeleteProduct(Guid id)
        {
            using (var conn = NewConnection())
            {
                conn.Open();
                var cmd = new SqliteCommand($"delete from Products where id = '{id}' collate nocase", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public IList<Product> SearchProducts(string name = null)
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
                using (var rdr = cmd.ExecuteReader())
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

        public SqliteConnection NewConnection()
        {
            return new SqliteConnection(this._configuration.GetConnectionString("xero"));
        }
    }
}