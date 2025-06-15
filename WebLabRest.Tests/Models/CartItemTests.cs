using WebLabRest.Models;
using Xunit;

namespace WebLabRest.Tests;

public class CartItemTests
{
    [Fact]
    public void CartItem_CreatesCorrectly_WithGivenDishAndQty()
    {
        // Arrange
        var dish = new Dish { Id = 1, Name = "Soup", Calories = 80 };

        // Act
        var item = new CartItem
        {
            Item = dish,
            Qty = 3
        };

        // Assert
        Assert.Equal(dish, item.Item);
        Assert.Equal(3, item.Qty);
    }

    [Fact]
    public void CartItem_AllowsPropertyModification()
    {
        var item = new CartItem();
        var dish = new Dish { Id = 2, Name = "Cake", Calories = 250 };

        item.Item = dish;
        item.Qty = 5;

        Assert.Equal(2, item.Item.Id);
        Assert.Equal("Cake", item.Item.Name);
        Assert.Equal(250, item.Item.Calories);
        Assert.Equal(5, item.Qty);
    }
}