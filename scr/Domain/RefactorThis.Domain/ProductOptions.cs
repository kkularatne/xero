using System.Collections.Generic;

namespace RefactorThis.Domain
{
    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions(List<ProductOption> items)
        {
            Items = items;
        }
    }
}