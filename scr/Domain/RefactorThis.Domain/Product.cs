using System;
//using System.Text.Json.Serialization;

namespace RefactorThis.Domain
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

       // [JsonIgnore] public bool IsNew { get; }

        public Product()
        {

        }
    }
}