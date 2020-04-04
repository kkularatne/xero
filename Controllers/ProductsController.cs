using System;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Models;
using RefactorThis.Repositories;
using RefactorThis.Services;
using SQLitePCL;

namespace RefactorThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;
        private readonly IProductOptionService _productOptionService;

        public ProductsController(IProductRepository productRepository, IProductService productService, IProductOptionService productOptionService)
        {
            _productRepository = productRepository;
            _productService = productService;
            _productOptionService = productOptionService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var products = _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            try
            {
                return Ok(_productService.GetProduct(id));
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Post(Product product)
        {
            try
            {
                var id = _productService.Save(product);
                return Created(new Uri("https://localhost:44335/api/products"), id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, Product product)
        {
            try
            {
                var originalProduct= _productService.GetProduct(id);
                _productService.Update(originalProduct.Id, product);
                return NoContent();

            }
            catch (RecordNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var originalProduct = _productService.GetProduct(id);
                _productService.Delete(originalProduct.Id);
                return NoContent();
            }
            catch (RecordNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        [HttpGet("{productId}/options")]
        public IActionResult GetOptions(Guid productId)
        {
            var productOptions = _productOptionService.GetProductOptionsByProductId(productId);
            return Ok(productOptions);
        }

        [HttpGet("{productId}/options/{id}")]
        public IActionResult GetOption(Guid productId, Guid id)
        {
            var product = _productService.GetProduct(productId);
            var productOption  =_productOptionService.GetProductOption(id);
            //var option = new ProductOption(id,_productRepository);
            //if (option.IsNew)
            //    throw new Exception();

            //return option;
            return Ok(productOption);
        }

        [HttpPost("{productId}/options")]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            option.Save();
        }

        [HttpPut("{productId}/options/{id}")]
        public void UpdateOption(Guid id, ProductOption option)
        {
            //var orig = new ProductOption(id,_productRepository)
            //{
            //    Name = option.Name,
            //    Description = option.Description
            //};

            //if (!orig.IsNew)
            //    orig.Save();
        }

        [HttpDelete("{productId}/options/{id}")]
        public IActionResult DeleteOption(Guid id)
        {
            _productOptionService.Delete(id);
            return NoContent();
        }
    }
}