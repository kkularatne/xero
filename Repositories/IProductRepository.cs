﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RefactorThis.Models;

namespace RefactorThis.Repositories
{
    public interface IProductRepository
    {
        Task<Product> SelectProductAsync(Guid id);
        Task SaveProductAsync(Guid id, string name, string description, decimal price, decimal deliveryPrice);
        Task UpdateProductAsync(Guid id, string name, string description, decimal price, decimal deliveryPrice);
        Task DeleteProductAsync(Guid id);

        Task<IList<Product>> SearchProductsAsync(string name = null);
    }
}