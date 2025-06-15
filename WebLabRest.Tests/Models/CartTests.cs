using WebLabRest.Models;
using Xunit;

namespace WebLabRest.Tests;

using Xunit;
using WebLabRest.Models;
using System.Collections.Generic;

public class CartTests
{
    private Cart _cart;

    public CartTests()
    {
        _cart = new Cart();
    }

    [Fact]
    public void AddToCart_AddsNewItem_WhenItemNotInCart()
    {
        var dish = new Dish { Id = 1, Calories = 100, Name = "Salad" };

        _cart.AddToCart(dish);

        Assert.Single(_cart.CartItems);
        Assert.Equal(1, _cart.CartItems[dish.Id].Qty);
        Assert.Equal(dish, _cart.CartItems[dish.Id].Item);
    }

    [Fact]
    public void AddToCart_IncrementsQty_WhenItemAlreadyInCart()
    {
        var dish = new Dish { Id = 1, Calories = 150, Name = "Burger" };

        _cart.AddToCart(dish);
        _cart.AddToCart(dish);

        Assert.Single(_cart.CartItems);
        Assert.Equal(2, _cart.CartItems[dish.Id].Qty);
    }

    [Fact]
    public void RemoveItems_RemovesItemById()
    {
        var dish = new Dish { Id = 2, Calories = 200 };
        _cart.AddToCart(dish);

        _cart.RemoveItems(2);

        Assert.Empty(_cart.CartItems);
    }

    [Fact]
    public void ClearAll_RemovesAllItems()
    {
        _cart.AddToCart(new Dish { Id = 1, Calories = 100 });
        _cart.AddToCart(new Dish { Id = 2, Calories = 150 });

        _cart.ClearAll();

        Assert.Empty(_cart.CartItems);
    }

    [Fact]
    public void Count_ReturnsTotalQuantityOfAllItems()
    {
        _cart.AddToCart(new Dish { Id = 1, Calories = 100 });
        _cart.AddToCart(new Dish { Id = 1, Calories = 100 }); // Qty = 2
        _cart.AddToCart(new Dish { Id = 2, Calories = 200 }); // Qty = 1

        Assert.Equal(3, _cart.Count);
    }

    [Fact]
    public void TotalCalories_ReturnsSumOfAllCalories()
    {
        _cart.AddToCart(new Dish { Id = 1, Calories = 100 }); // x2 = 200
        _cart.AddToCart(new Dish { Id = 1, Calories = 100 });
        _cart.AddToCart(new Dish { Id = 2, Calories = 300 }); // x1 = 300

        Assert.Equal(500, _cart.TotalCalories);
    }
}
