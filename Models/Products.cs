using System;
using System.Collections.Generic;
using RefactorThis.Repositories;

namespace RefactorThis.Models
{
    public interface IProducts
    {
        List<Product> Items { get; }
    }

    public class Products : IProducts
    {
        public List<Product> Items { get; }

        public Products(List<Product> items)
        {
            Items = items;
        }
    }
}