using System;
using System.Linq;
using RefactorThis.Models;
using RefactorThis.Repositories;

namespace RefactorThis.Services
{
    public interface IProductService
    {
        Products GetAllProducts();
        Products GetProductsByName(string name);

        Product GetProduct(Guid id);

        void Save(Product product);
        void Update(Product product);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Products GetAllProducts()
        {
           var products = _productRepository.SearchProducts(string.Empty);
           return new Products(products.ToList());
        }

        public Products GetProductsByName(string name)
        {
            var products = _productRepository.SearchProducts(name);
            return new Products(products.ToList());
        }

        public Product GetProduct(Guid id)
        {
            return _productRepository.SelectProduct(id);
        }

        public void Save(Product product)
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
