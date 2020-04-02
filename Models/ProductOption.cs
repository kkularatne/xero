using System;
using Newtonsoft.Json;

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
        private readonly IHelpers _helpers;

        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore] public bool IsNew { get; }

        public ProductOption(IHelpers helpers)
        {
            _helpers = helpers;
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id, IHelpers helpers)
        {
            _helpers = helpers;
            IsNew = true;
            var conn = _helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = $"select * from productoptions where id = '{id}' collate nocase";

            var rdr = cmd.ExecuteReader();
            if (!rdr.Read())
                return;

            IsNew = false;
            Id = Guid.Parse(rdr["Id"].ToString());
            ProductId = Guid.Parse(rdr["ProductId"].ToString());
            Name = rdr["Name"].ToString();
            Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
        }

        public void Save()
        {
            var conn = _helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = IsNew
                ? $"insert into productoptions (id, productid, name, description) values ('{Id}', '{ProductId}', '{Name}', '{Description}')"
                : $"update productoptions set name = '{Name}', description = '{Description}' where id = '{Id}' collate nocase";

            cmd.ExecuteNonQuery();
        }

        public void Delete()
        {
            var conn = _helpers.NewConnection();
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = $"delete from productoptions where id = '{Id}' collate nocase";
            cmd.ExecuteReader();
        }
    }
}