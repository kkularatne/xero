﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RefactorThis.Models;
using RefactorThis.Repositories;

namespace RefactorThis.Services
{
    public interface IProductOptionService
    {
        ProductOptions GetAllProductOptions();
        ProductOptions GetProductOptionsByProductId(Guid productId);
        ProductOption GetProductOption(Guid id);
        Guid Save(Guid productId, ProductOption option);
        void Update(Guid id, ProductOption option);
        void Delete(Guid id);
    }

    public class ProductOptionService : IProductOptionService
    {
        private readonly IProductOptionRepository _productOptionRepository;

        public ProductOptionService(IProductOptionRepository productOptionRepository)
        {
            _productOptionRepository = productOptionRepository;
        }

        public ProductOptions GetAllProductOptions()
        {
            var productOptions = _productOptionRepository.SearchProductOptions(string.Empty);
            return new ProductOptions(productOptions.ToList());
        }

        public ProductOptions GetProductOptionsByProductId(Guid productId)
        {
            var productOptions = _productOptionRepository.SearchProductOptions(productId.ToString());
            return new ProductOptions(productOptions.ToList());
        }

        public ProductOption GetProductOption(Guid id)
        {
            return _productOptionRepository.SelectProductOption(id);
        }

        public Guid Save(Guid productId, ProductOption option)
        {
            var id = Guid.NewGuid();
            _productOptionRepository.SaveProductOption(id, productId, option.Name, option.Description);
            return id;
        }

        public void Update(Guid id, ProductOption option)
        {
            _productOptionRepository.UpdateProductOption(id,option.Name,option.Description);
        }

        public void Delete(Guid id)
        {
            _productOptionRepository.DeleteProductOption(id);
        }
    }
}
