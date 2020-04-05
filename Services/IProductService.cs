using System;
using RefactorThis.Models;

namespace RefactorThis.Services
{
    public interface IProductService
    {
        Products GetAllProducts();
        Products GetProductsByName(string name);
        Product GetProduct(Guid id);
        Guid Save(Product product);
        void Update(Guid id, Product product);
        void Delete(Guid id);
    }
}