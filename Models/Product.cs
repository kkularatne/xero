using System;
using Newtonsoft.Json;
using RefactorThis.Repositories;

namespace RefactorThis.Models
{
    public interface IProduct
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        decimal Price { get; set; }
        decimal DeliveryPrice { get; set; }
        bool IsNew { get; }
        void Save();
        void Delete();
    }

    public class Product : IProduct
    {
        private readonly IProductRepository _productRepository;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore] public bool IsNew { get; }

        //public Product(IProductRepository productRepository)
        //{
        //    _productRepository = productRepository;
        //    Id = Guid.NewGuid();
        //    IsNew = true;
        //}

        public Product()
        {
            
        }

        public Product(Guid id, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            try
            {
               var product = _productRepository.SelectProduct(id);
               Id = product.Id;
               Name = product.Name;
               Description = product.Description;
               Price = product.Price;
               DeliveryPrice = product.DeliveryPrice;
            }
            catch (RecordNotFoundException e)
            {
                IsNew = true;
                return;
            }
        }

        public void Save()
        {
            if (IsNew)
                _productRepository.SaveProduct(Id, Name, Description, Price, DeliveryPrice);
            else
                _productRepository.UpdateProduct(Id, Name, Description, Price, DeliveryPrice);
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id,_productRepository).Items)
                option.Delete();

            _productRepository.DeleteProduct(Id);
        }
    }
}