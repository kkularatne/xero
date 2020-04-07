using System.Collections.Generic;

namespace RefactorThis.Domain
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