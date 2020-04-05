using System.Collections.Generic;

namespace RefactorThis.Models
{
    public class Products
    {
        public List<Product> Items { get; }

        public Products(List<Product> items)
        {
            Items = items;
        }
    }
}