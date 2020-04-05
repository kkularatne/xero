using System;
using System.Collections.Generic;
using RefactorThis.Models;

namespace RefactorThis.Repositories
{
    public interface IProductOptionRepository
    {
        IList<ProductOption> SearchProductOptions(string productId = null);
        ProductOption SelectProductOption(Guid id);
        void SaveProductOption(Guid id, Guid productId, string name, string description);
        void UpdateProductOption(Guid id, string name, string description);
        void DeleteProductOption(Guid id);
    }
}