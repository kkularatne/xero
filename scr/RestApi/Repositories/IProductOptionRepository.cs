using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RefactorThis.Domain;

namespace RefactorThis.Repositories
{
    public interface IProductOptionRepository
    {
        Task<IList<ProductOption>> SearchProductOptionsAsync(string productId = null);
        Task<ProductOption> SelectProductOptionAsync(Guid id);
        Task SaveProductOptionAsync(Guid id, Guid productId, string name, string description);
        Task UpdateProductOptionAsync(Guid id, string name, string description);
        Task DeleteProductOptionAsync(Guid id);
    }
}