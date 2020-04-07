using System;
using System.Linq;
using System.Threading.Tasks;
using RefactorThis.Domain;
using RefactorThis.Repositories;

namespace RefactorThis.Services
{
    public class ProductOptionService : IProductOptionService
    {
        private readonly IProductOptionRepository _productOptionRepository;

        public ProductOptionService(IProductOptionRepository productOptionRepository)
        {
            _productOptionRepository = productOptionRepository;
        }

        public async Task<ProductOptions> GetProductOptionsByProductIdAsync(Guid productId)
        {
            var productOptions = await _productOptionRepository.SearchProductOptionsAsync(productId.ToString());
            return new ProductOptions(productOptions.ToList());
        }

        public async Task<ProductOption> GetProductOptionAsync(Guid id)
        {
            return await _productOptionRepository.SelectProductOptionAsync(id);
        }

        public async Task<Guid> SaveAsync(Guid productId, ProductOption option)
        {
            var id = Guid.NewGuid();
            await _productOptionRepository.SaveProductOptionAsync(id, productId, option.Name, option.Description);
            return id;
        }

        public async Task UpdateAsync(Guid id, ProductOption option)
        {
            await _productOptionRepository.UpdateProductOptionAsync(id,option.Name,option.Description);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _productOptionRepository.DeleteProductOptionAsync(id);
        }
    }
}
