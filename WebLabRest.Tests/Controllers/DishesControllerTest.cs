using JetBrains.Annotations;
using WebLabRest.Controllers;
using WebLabRest.UI.Services;

namespace WebLabRest.Tests.Controllers;

using Xunit;
using Microsoft.EntityFrameworkCore;
using WebLabRest.Controllers;
using WebLabRest.Data;
using WebLabRest.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class DishesControllerTests
{
    private AppDbContext GetInMemoryDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        return new AppDbContext(options);
    }

    private Mock<IWebHostEnvironment> GetMockEnv()
    {
        var mockEnv = new Mock<IWebHostEnvironment>();
        mockEnv.Setup(m => m.WebRootPath).Returns(System.IO.Path.GetTempPath());
        return mockEnv;
    }

    [Fact]
    public async Task GetDishes_ReturnsPaginatedDishes()
    {
        // Arrange
        var context = GetInMemoryDbContext(nameof(GetDishes_ReturnsPaginatedDishes));
        var category = new Category { Id = 1, Name = "Drinks", NormalizedName = "DRINKS" };
        context.Categories.Add(category);
        context.Dishes.AddRange(
            new Dish { Id = 1, Name = "Water", Calories = 0, Category = category },
            new Dish { Id = 2, Name = "Juice", Calories = 50, Category = category },
            new Dish { Id = 3, Name = "Coffee", Calories = 5, Category = category },
            new Dish { Id = 4, Name = "Tea", Calories = 5, Category = category }
        );
        await context.SaveChangesAsync();

        var controller = new DishesController(context, GetMockEnv().Object);

        // Act
        var result = await controller.GetDishes("DRINKS", pageNo: 1, pageSize: 3);

        // Assert
        var actionResult = Assert.IsType<ActionResult<ResponseData<ListModel<Dish>>>>(result);
        var responseData = Assert.IsType<ResponseData<ListModel<Dish>>>(actionResult.Value);
        Assert.Equal(3, responseData.Data.Items.Count);
        Assert.Equal(1, responseData.Data.CurrentPage);
        Assert.Equal(2, responseData.Data.TotalPages);
    }

    [Fact]
    public async Task GetDish_ReturnsDish_WhenExists()
    {
        // Arrange
        var context = GetInMemoryDbContext(nameof(GetDish_ReturnsDish_WhenExists));
        var category = new Category { Id = 1, Name = "Desserts", NormalizedName = "DESSERTS" };
        context.Categories.Add(category);
        var dish = new Dish { Id = 1, Name = "Cake", Calories = 400, Category = category };
        context.Dishes.Add(dish);
        await context.SaveChangesAsync();

        var controller = new DishesController(context, GetMockEnv().Object);

        // Act
        var result = await controller.GetDish(1);

        // Assert
        var actionResult = Assert.IsType<ActionResult<ResponseData<Dish>>>(result);
        var responseData = Assert.IsType<ResponseData<Dish>>(actionResult.Value);
        Assert.NotNull(responseData.Data);
        Assert.Equal("Cake", responseData.Data.Name);
    }

    [Fact]
    public async Task GetDish_ReturnsNotFound_WhenNotExists()
    {
        // Arrange
        var context = GetInMemoryDbContext(nameof(GetDish_ReturnsNotFound_WhenNotExists));
        var controller = new DishesController(context, GetMockEnv().Object);

        // Act
        var result = await controller.GetDish(999);

        // Assert
        var actionResult = Assert.IsType<ActionResult<ResponseData<Dish>>>(result);
        Assert.IsType<NotFoundObjectResult>(actionResult.Result);
    }

    [Fact]
    public async Task PostDish_AddsDishSuccessfully()
    {
        // Arrange
        var context = GetInMemoryDbContext(nameof(PostDish_AddsDishSuccessfully));
        var category = new Category { Id = 1, Name = "Main", NormalizedName = "MAIN" };
        context.Categories.Add(category);
        await context.SaveChangesAsync();

        var controller = new DishesController(context, GetMockEnv().Object);

        var newDish = new Dish { Name = "Steak", Calories = 700, CategoryId = 1 };

        // Act
        var result = await controller.PostDish(newDish);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var responseData = Assert.IsType<ResponseData<Dish>>(createdResult.Value);
        Assert.Equal("Steak", responseData.Data.Name);

        Assert.Equal(1, context.Dishes.Count());
    }

    [Fact]
    public async Task DeleteDish_RemovesDish_WhenExists()
    {
        // Arrange
        var context = GetInMemoryDbContext(nameof(DeleteDish_RemovesDish_WhenExists));
        var category = new Category { Id = 1, Name = "Soup", NormalizedName = "SOUP" };
        context.Categories.Add(category);
        var dish = new Dish { Id = 1, Name = "Borscht", Calories = 200, Category = category };
        context.Dishes.Add(dish);
        await context.SaveChangesAsync();

        var controller = new DishesController(context, GetMockEnv().Object);

        // Act
        var result = await controller.DeleteDish(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Empty(context.Dishes);
    }

    [Fact]
    public async Task DeleteDish_ReturnsNotFound_WhenDishDoesNotExist()
    {
        // Arrange
        var context = GetInMemoryDbContext(nameof(DeleteDish_ReturnsNotFound_WhenDishDoesNotExist));
        var controller = new DishesController(context, GetMockEnv().Object);

        // Act
        var result = await controller.DeleteDish(123);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
