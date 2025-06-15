using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebLabRest.Data;
using WebLabRest.Models;
using Xunit;

public class AppDbContextTests
{
    [Fact]
    public void CanAddAndRetrieveCategory()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb1")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var category = new Category { Id = 1, Name = "Drinks", NormalizedName = "DRINKS" };
            context.Categories.Add(category);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(options))
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == 1);
            Assert.NotNull(category);
            Assert.Equal("Drinks", category.Name);
        }
    }

    [Fact]
    public void CanAddAndRetrieveDish()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb2")
            .Options;

        using (var context = new AppDbContext(options))
        {
            var dish = new Dish { Id = 1, Name = "Pizza", Calories = 800 };
            context.Dishes.Add(dish);
            context.SaveChanges();
        }

        using (var context = new AppDbContext(options))
        {
            var dish = context.Dishes.FirstOrDefault(d => d.Id == 1);
            Assert.NotNull(dish);
            Assert.Equal("Pizza", dish.Name);
        }
    }
}
