using System;
using System.Threading.Tasks;
using RefactorThis.Domain;

namespace RefactorThis.Services
{
    public interface IProductService
    {
        Task<Products> GetAllProductsAsync();
        Task<Products> GetProductsByNameAsync(string name);
        Task<Product> GetProductAsync(Guid id);
        Task<Guid> SaveAsync(Product product);
        Task UpdateAsync(Guid id, Product product);
        Task DeleteAsync(Guid id);
    }
}