using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using RefactorThis.Models;

namespace RefactorThis.Repositories
{
    public interface IProductRepository
    {
        Product SelectProduct(Guid id);
        void SaveProduct(Guid id, string name, string description, decimal price, decimal deliveryPrice);
        void UpdateProduct(Guid id, string name, string description, decimal price, decimal deliveryPrice);
        void DeleteProduct(Guid id);

        IList<Product> SearchProducts(string name);
    }
}