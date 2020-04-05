using System;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Constants;
using RefactorThis.Models;
using RefactorThis.Repositories;
using RefactorThis.Services;
using Serilog;

namespace RefactorThis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IProductOptionService _productOptionService;

        public ProductsController(IProductRepository productRepository, IProductService productService, IProductOptionService productOptionService)
        {
            _productService = productService;
            _productOptionService = productOptionService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var products = _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }
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
                Log.Error(e, ExceptionTemplates.RecordNotFoundError);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
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
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
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
                Log.Error(e, ExceptionTemplates.RecordNotFoundError);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
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
                Log.Error(e, ExceptionTemplates.RecordNotFoundError);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }

        }

        [HttpGet("{productId}/options")]
        public IActionResult GetOptions(Guid productId)
        {
            try
            {
                var productOptions = _productOptionService.GetProductOptionsByProductId(productId);
                return Ok(productOptions);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }
        }

        [HttpGet("{productId}/options/{id}")]
        public IActionResult GetOption(Guid productId, Guid id)
        {
            try
            {
                _productService.GetProduct(productId);
                var productOption = _productOptionService.GetProductOption(id);
                return Ok(productOption);
            }
            catch (RecordNotFoundException e)
            {
                Log.Error(e, ExceptionTemplates.RecordNotFoundError);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }
        }

        [HttpPost("{productId}/options")]
        public IActionResult CreateOption(Guid productId, ProductOption option)
        {
            try
            {
                _productService.GetProduct(productId);
                var id = _productOptionService.Save(productId, option);
                return Created(string.Empty, id);
            }
            catch (RecordNotFoundException e)
            {
                Log.Error(e, ExceptionTemplates.RecordNotFoundError);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }
        }

        [HttpPut("{productId}/options/{id}")]
        public IActionResult UpdateOption(Guid id, ProductOption option)
        {
            try
            {
                _productOptionService.GetProductOption(id);
                _productOptionService.Update(id, option);
                return NoContent();
            }
            catch (RecordNotFoundException e)
            {
                Log.Error(e, ExceptionTemplates.RecordNotFoundError);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }
        }

        [HttpDelete("{productId}/options/{id}")]
        public IActionResult DeleteOption(Guid id)
        {
            try
            {
                _productOptionService.GetProductOption(id);
                _productOptionService.Delete(id);
                return NoContent();
            }
            catch (RecordNotFoundException e)
            {
                Log.Error(e, ExceptionTemplates.RecordNotFoundError);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }
        }
    }
}