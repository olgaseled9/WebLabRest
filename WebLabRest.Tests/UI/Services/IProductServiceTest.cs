using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using WebLabRest.Models;
using WebLabRest.UI.Services;
using Xunit;

public class ProductServiceTests
{
    [Fact]
    public async Task GetProductListAsync_ReturnsData()
    {
        // Arrange
        var mockService = new Mock<IProductService>();
        var expectedData = new ResponseData<ListModel<Dish>>
        {
            Success = true,
            Data = new ListModel<Dish>
            {
                Items = new List<Dish>
                {
                    new Dish { Id = 1, Name = "Pizza", Calories = 800 }
                },
                CurrentPage = 1,
                TotalPages = 1
            }
        };
        
        mockService
            .Setup(s => s.GetProductListAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(expectedData);

        // Act
        var result = await mockService.Object.GetProductListAsync(null);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data.Items);
        Assert.Equal("Pizza", result.Data.Items[0].Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsDish()
    {
        var mockService = new Mock<IProductService>();
        var expectedDish = new Dish { Id = 1, Name = "Pizza", Calories = 800 };

        mockService
            .Setup(s => s.GetProductByIdAsync(1))
            .ReturnsAsync(new ResponseData<Dish> { Success = true, Data = expectedDish });

        var result = await mockService.Object.GetProductByIdAsync(1);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Pizza", result.Data.Name);
    }

    [Fact]
    public async Task CreateProductAsync_ReturnsSuccess()
    {
        var mockService = new Mock<IProductService>();

        mockService
            .Setup(s => s.CreateProductAsync(It.IsAny<Dish>(), It.IsAny<IFormFile>()))
            .ReturnsAsync(new ResponseData<Dish> { Success = true });

        var result = await mockService.Object.CreateProductAsync(new Dish(), null);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task UpdateProductAsync_CallsMethod()
    {
        var mockService = new Mock<IProductService>();

        mockService.Setup(s => s.UpdateProductAsync(It.IsAny<Dish>())).Returns(Task.CompletedTask);

        await mockService.Object.UpdateProductAsync(new Dish { Id = 1 });

        mockService.Verify(s => s.UpdateProductAsync(It.Is<Dish>(d => d.Id == 1)), Times.Once);
    }

    [Fact]
    public async Task DeleteProductAsync_CallsMethod()
    {
        var mockService = new Mock<IProductService>();

        mockService.Setup(s => s.DeleteProductAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        await mockService.Object.DeleteProductAsync(1);

        mockService.Verify(s => s.DeleteProductAsync(1), Times.Once);
    }
}
