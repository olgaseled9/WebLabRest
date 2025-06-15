using WebLabRest.Models;
using Xunit;

namespace WebLabRest.Tests;

public class CartTests
{
    [Fact]
    public void AddToCart_NewItem_AddsItemWithQuantity1()
    {
        // Arrange
        var cart = new Cart();
        var dish = new Dish { Id = 1 };

        // Act
        cart.AddToCart(dish);

        // Assert
        Assert.Equal(1, cart.Count);
        Assert.Equal(1, cart.CartItems[1].Qty);
    }

    [Fact]
    public void AddToCart_ExistingItem_IncrementsQuantity()
    {
        // Arrange
        var cart = new Cart();
        var dish = new Dish { Id = 1 };
        cart.AddToCart(dish);

        // Act
        cart.AddToCart(dish);

        // Assert
        Assert.Equal(1, cart.CartItems.Count);
        Assert.Equal(2, cart.CartItems[1].Qty);
    }

    [Fact]
    public void RemoveItems_ExistingItem_RemovesItem()
    {
        // Arrange
        var cart = new Cart();
        var dish = new Dish { Id = 1 };
        cart.AddToCart(dish);

        // Act
        cart.RemoveItems(1);

        // Assert
        Assert.Empty(cart.CartItems);
    }

    [Fact]
    public void ClearAll_WithItems_EmptiesCart()
    {
        // Arrange
        var cart = new Cart();
        cart.AddToCart(new Dish { Id = 1 });
        cart.AddToCart(new Dish { Id = 2 });

        // Act
        cart.ClearAll();

        // Assert
        Assert.Empty(cart.CartItems);
    }

    [Fact]
    public void TotalCalories_CalculatesCorrectly()
    {
        // Arrange
        var cart = new Cart();
        cart.AddToCart(new Dish { Id = 1, Calories = 100 });
        cart.AddToCart(new Dish { Id = 2, Calories = 200 });
        cart.AddToCart(new Dish { Id = 1 }); // Duplicate

        // Act & Assert
        Assert.Equal(400, cart.TotalCalories);
    }
}