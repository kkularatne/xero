using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RefactorThis.Constants;
using RefactorThis.Domain;
using RefactorThis.Repository;
using RefactorThis.Service;
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
        public async Task<IActionResult> GetAsync(string name)
        {
            try
            {
                var products = string.IsNullOrEmpty(name)
                    ? await _productService.GetAllProductsAsync()
                    : await _productService.GetProductsByNameAsync(name);
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
        public async Task<IActionResult> Post( Product product)
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
        public async Task<IActionResult> GetOptions(Guid productId)
        {
            try
            {
                var productOptions = await _productOptionService.GetProductOptionsByProductIdAsync(productId);
                return Ok(productOptions);
            }
            catch (Exception e)
            {
                Log.Error(e, ExceptionTemplates.UnknownError);
                return StatusCode(500);
            }
        }

        [HttpGet("{productId}/options/{id}")]
        public async Task<IActionResult> GetOption(Guid productId, Guid id)
        {
            try
            {
                await _productService.GetProductAsync(productId);
                var productOption = await _productOptionService.GetProductOptionAsync(id);
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
        public async Task<IActionResult> CreateOption(Guid productId, ProductOption option)
        {
            try
            {
                await _productService.GetProductAsync(productId);
                var id = await _productOptionService.SaveAsync(productId, option);
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
        public async Task<IActionResult> UpdateOption(Guid id, ProductOption option)
        {
            try
            {
                await _productOptionService.GetProductOptionAsync(id);
                await _productOptionService.UpdateAsync(id, option);
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
        public async Task<IActionResult> DeleteOption(Guid id)
        {
            try
            {
                await _productOptionService.GetProductOptionAsync(id);
                await _productOptionService.DeleteAsync(id);
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