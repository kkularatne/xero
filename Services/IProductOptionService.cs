using System;
using RefactorThis.Models;

namespace RefactorThis.Services
{
    public interface IProductOptionService
    {
        ProductOptions GetProductOptionsByProductId(Guid productId);
        ProductOption GetProductOption(Guid id);
        Guid Save(Guid productId, ProductOption option);
        void Update(Guid id, ProductOption option);
        void Delete(Guid id);
    }
}