using JetBrains.Annotations;
using WebLabRest.Controllers;

namespace WebLabRest.Tests.Controllers;

using Xunit;
using Microsoft.EntityFrameworkCore;
using WebLabRest.Controllers;
using WebLabRest.Data;
using WebLabRest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class CategoriesControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestCategoriesDb")
            .Options;
        var context = new AppDbContext(options);

        // Очистим базу перед каждым тестом
        context.Categories.RemoveRange(context.Categories);
        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task GetCategories_ReturnsAllCategories()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.Categories.AddRange(
            new Category { Id = 1, Name = "Drinks", NormalizedName = "DRINKS" },
            new Category { Id = 2, Name = "Desserts", NormalizedName = "DESSERTS" }
        );
        context.SaveChanges();

        var controller = new CategoriesController(context);

        // Act
        var result = await controller.GetCategories();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Category>>(actionResult.Value);
        Assert.Equal(2, returnValue.Count());
        Assert.Contains(returnValue, c => c.Name == "Drinks");
        Assert.Contains(returnValue, c => c.Name == "Desserts");
    }
}
