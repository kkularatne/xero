using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RefactorThis.Controllers;
using RefactorThis.Domain;
using RefactorThis.Repository;
using RefactorThis.Service;
using Xunit;

namespace XUnitTestProject
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetAsync_GetAllProducts()
        {
            // Arrange
            var mockIProductService = new Mock<IProductService>();
            mockIProductService
                .Setup(arg => arg.GetAllProductsAsync())
                .ReturnsAsync(new Products(new List<Product>()));
            mockIProductService
                .Setup(arg => arg.GetProductsByNameAsync(String.Empty))
                .ReturnsAsync(new Products(new List<Product>()));

            var mockIProductOptionService = new Mock<IProductOptionService>();

            var sut = new ProductsController(mockIProductService.Object, mockIProductOptionService.Object);

            // Act
            var response = await sut.GetAsync(string.Empty);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var objectResponse = response as ObjectResult;
            Assert.Equal(200, objectResponse.StatusCode);
            mockIProductService.Verify(arg => arg.GetAllProductsAsync(), Times.Once);
            mockIProductService.Verify(arg => arg.GetProductsByNameAsync(String.Empty), Times.Never);
        }

        [Fact]
        public async Task GetAsync_GetProductsByName()
        {
            // Arrange
            var mockIProductService = new Mock<IProductService>();
            mockIProductService
                .Setup(arg => arg.GetAllProductsAsync())
                .ReturnsAsync(new Products(new List<Product>()));
            mockIProductService
                .Setup(arg => arg.GetProductsByNameAsync("TEST"))
                .ReturnsAsync(new Products(new List<Product>()));

            var mockIProductOptionService = new Mock<IProductOptionService>();

            var sut = new ProductsController(mockIProductService.Object, mockIProductOptionService.Object);

            // Act
            var response = await sut.GetAsync("TEST");

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var objectResponse = response as ObjectResult;
            Assert.Equal(200, objectResponse.StatusCode);
            mockIProductService.Verify(arg => arg.GetAllProductsAsync(), Times.Never);
            mockIProductService.Verify(arg => arg.GetProductsByNameAsync("TEST"), Times.Once);
        }

        [Fact]
        public async Task GetAsync_ProductById()
        {
            // Arrange
            var guid = new Guid();
            //var recordNotFoundId = new Guid();
            //var exceptionId = new Guid();

            var mockIProductService = new Mock<IProductService>();
            mockIProductService
            .Setup(arg => arg.GetProductAsync(guid))
                .ReturnsAsync(new Product());
            
            var mockIProductOptionService = new Mock<IProductOptionService>();

            var sut = new ProductsController(mockIProductService.Object, mockIProductOptionService.Object);

            // Act
            var response = await  sut.Get(guid);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var objectResponse = response as ObjectResult;
            Assert.Equal(200, objectResponse.StatusCode);

            // Arrange
            mockIProductService.Setup(arg => arg.GetProductAsync(guid))
                .ThrowsAsync(new RecordNotFoundException(guid, "product"));

            // Act
            response = await sut.Get(guid);

            // Assert
            Assert.IsType<NotFoundObjectResult>(response);
            objectResponse = response as ObjectResult;
            Assert.Equal(404, objectResponse.StatusCode);
            Assert.Equal($"product not found for the Id {guid}", objectResponse.Value);

            // Arrange
            mockIProductService.Setup(arg => arg.GetProductAsync(guid))
                .ThrowsAsync(new Exception());

            // Act
            response = await sut.Get(guid);

            // Assert
            Assert.IsType<StatusCodeResult>(response);
            Assert.Equal(500, ((StatusCodeResult)response).StatusCode);
        }
    }
}
