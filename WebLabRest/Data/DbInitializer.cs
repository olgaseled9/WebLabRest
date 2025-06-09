using Microsoft.EntityFrameworkCore;
using WebLabRest.Data;
using WebLabRest.Models;

namespace WebLabRest.Data;

public static class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.MigrateAsync();

        if (!context.Categories.Any() && !context.Dishes.Any())
        {
            var uri = "https://localhost:7002/";

            var categories = new List<Category>
            {
                new() { Name = "Супы", NormalizedName = "soups" },
                new() { Name = "Основные блюда", NormalizedName = "main-dishes" },
                new() { Name = "Напитки", NormalizedName = "drinks" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var dishes = new List<Dish>
            {
                new() {
                    Name = "Суп-харчо",
                    Description = "Очень острый, невкусный",
                    Calories = 200,
                    Category = categories[0],
                    Image = uri + "Images/soup.jpg"
                },
                new() {
                    Name = "Борщ",
                    Description = "Много сала, без сметаны",
                    Calories = 330,
                    Category = categories[0],
                    Image = uri + "Images/borscht.jpg"
                },
                new() {
                    Name = "Котлета пожарская",
                    Description = "Хлеб 80%, морковь 20%",
                    Calories = 635,
                    Category = categories[1],
                    Image = uri + "Images/cutlet.jpg"
                },
                new() {
                    Name = "Компот",
                    Description = "Быстрорастворимый, 2 литра",
                    Calories = 180,
                    Category = categories[2],
                    Image = uri + "Images/kompot.jpg"
                }
            };

            await context.Dishes.AddRangeAsync(dishes);
            await context.SaveChangesAsync();
        }
    }
}
