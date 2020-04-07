using System;
using System.Threading.Tasks;
using RefactorThis.Models;

namespace RefactorThis.Services
{
    public interface IProductOptionService
    {
        Task<ProductOptions> GetProductOptionsByProductIdAsync(Guid productId);
        Task<ProductOption> GetProductOptionAsync(Guid id);
        Task<Guid> SaveAsync(Guid productId, ProductOption option);
        Task UpdateAsync(Guid id, ProductOption option);
        Task DeleteAsync(Guid id);
    }
}