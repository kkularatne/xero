using System;
using System.Threading.Tasks;
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

        public ProductsController(IProductService productService, IProductOptionService productOptionService)
        {
            _productService = productService;
            _productOptionService = productOptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                return Ok(await _productService.GetProductAsync(id));
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
        public async Task<IActionResult> Post(Product product)
        {
            try
            {
                var id = await _productService.SaveAsync(product);
                return Created(new Uri("https://localhost:44335/api/products"), id);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, Product product)
        {
            try
            {
                var originalProduct= await _productService.GetProductAsync(id);
                await _productService.UpdateAsync(originalProduct.Id, product);
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
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var originalProduct = await _productService.GetProductAsync(id);
                await _productService.DeleteAsync(originalProduct.Id);
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