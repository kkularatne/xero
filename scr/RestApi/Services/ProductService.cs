using System;
using System.Linq;
using System.Threading.Tasks;
using RefactorThis.Domain;
using RefactorThis.Repository;

namespace RefactorThis.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Products> GetAllProductsAsync()
        {
            var products = await _productRepository.SearchProductsAsync(string.Empty);
            return new Products(products.ToList());
        }

        public async Task<Products> GetProductsByNameAsync(string name)
        {
            var products = await _productRepository.SearchProductsAsync(name);
            return new Products(products.ToList());
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            return await _productRepository.SelectProductAsync(id);
        }

        public async Task<Guid> SaveAsync(Product product)
        {
            var id = Guid.NewGuid();
            await _productRepository.SaveProductAsync(id,
                product.Name,
                product.Description,
                product.Price,
                product.DeliveryPrice);
            return id;
        }

        public async Task UpdateAsync(Guid id, Product product)
        {
            await _productRepository.UpdateProductAsync(id,
                product.Name,
                product.Description,
                product.Price,
                product.DeliveryPrice);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
