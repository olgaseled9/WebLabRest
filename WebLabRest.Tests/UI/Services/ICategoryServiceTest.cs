using JetBrains.Annotations;
using WebLabRest.UI.Services;

namespace WebLabRest.Tests.UI.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using WebLabRest.Models;
using WebLabRest.UI.Services;
using Xunit;

public class CategoryServiceTests
{
    [Fact]
    public async Task GetCategoryListAsync_ReturnsCategories()
    {
        // Arrange
        var mockCategoryService = new Mock<ICategoryService>();

        var expectedCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Drinks", NormalizedName = "DRINKS" },
            new Category { Id = 2, Name = "Desserts", NormalizedName = "DESSERTS" }
        };

        mockCategoryService
            .Setup(service => service.GetCategoryListAsync())
            .ReturnsAsync(new ResponseData<List<Category>>
            {
                Success = true,
                Data = expectedCategories
            });

        // Act
        var result = await mockCategoryService.Object.GetCategoryListAsync();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal("Drinks", result.Data[0].Name);
        Assert.Equal("Desserts", result.Data[1].Name);
    }
}
