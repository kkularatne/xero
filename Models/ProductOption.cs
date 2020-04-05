using System;
using Newtonsoft.Json;
using RefactorThis.Repositories;

namespace RefactorThis.Models
{
    public interface IProductOption
    {
        Guid Id { get; set; }
        Guid ProductId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        bool IsNew { get; }
        void Save();
        void Delete();
    }

    public class ProductOption : IProductOption
    {
        private readonly IProductRepository _productRepository;

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore] public bool IsNew { get; }

        public ProductOption(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption()
        {
            //_productRepository = productRepository;
            //IsNew = true;
            //var conn = _productRepository.NewConnection();
            //conn.Open();
            //var cmd = conn.CreateCommand();

            //cmd.CommandText = $"select * from productoptions where id = '{id}' collate nocase";

            //var rdr = cmd.ExecuteReader();
            //if (!rdr.Read())
            //    return;

            //IsNew = false;
            //Id = Guid.Parse(rdr["Id"].ToString());
            //ProductId = Guid.Parse(rdr["ProductId"].ToString());
            //Name = rdr["Name"].ToString();
            //Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
        }

        public void Save()
        {
            //var conn = _productRepository.NewConnection();
            //conn.Open();
            //var cmd = conn.CreateCommand();

            //cmd.CommandText = IsNew
            //    
            //    : ;

            //cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            //var conn = _productRepository.NewConnection();
            //conn.Open();
            //var cmd = conn.CreateCommand();
            //cmd.CommandText = $"delete from productoptions where id = '{Id}' collate nocase";
            //cmd.ExecuteReader();
        }
    }
}