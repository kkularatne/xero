using System;
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
            var productOptions = _productOptionRepository.SearchProducts(string.Empty);
            return new ProductOptions(productOptions.ToList());
        }

        public ProductOptions GetProductOptionsByProductId(Guid productId)
        {
            var productOptions = _productOptionRepository.SearchProducts(productId.ToString());
            return new ProductOptions(productOptions.ToList());
        }

        public ProductOption GetProductOption(Guid id)
        {
            return _productOptionRepository.SelectProductOption(id);
        }

        public void Delete(Guid id)
        {
            _productOptionRepository.DeleteProductOption(id);
        }
    }
}
