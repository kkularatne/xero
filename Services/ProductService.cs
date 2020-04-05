using System;
using System.Linq;
using RefactorThis.Models;
using RefactorThis.Repositories;

namespace RefactorThis.Services
{
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

        public Guid Save(Product product)
        {
            var id = Guid.NewGuid();
            _productRepository.SaveProduct(id,
                product.Name,
                product.Description,
                product.Price,
                product.DeliveryPrice);
            return id;
        }

        public void Update(Guid id, Product product)
        {
            _productRepository.UpdateProduct(id,
                product.Name,
                product.Description,
                product.Price,
                product.DeliveryPrice);
        }

        public void Delete(Guid id)
        {
            _productRepository.DeleteProduct(id);
        }
    }
}
