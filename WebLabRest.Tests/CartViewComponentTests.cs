using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using WebLabRest.Controllers;
using WebLabRest.Models;
using Xunit;

namespace WebLabRest.Tests;

public class CartViewComponentTests
{
    [Fact]
    public void Invoke_EmptyCart_ReturnsEmptyView()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var component = new CartViewComponent { ViewComponentContext = new ViewComponentContext() };
        component.ViewComponentContext.ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext();
        component.ViewComponentContext.ViewContext.HttpContext = httpContext;

        // Act
        var result = component.Invoke();

        // Assert
        var viewResult = Assert.IsType<ViewViewComponentResult>(result);
        Assert.Null(viewResult.ViewData.Model);
    }

    [Fact]
    public void Invoke_WithCart_ReturnsCartModel()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var cart = new Cart();
        cart.AddToCart(new Dish { Id = 1 });
        httpContext.Session.Set("cart", cart);
            
        var component = new CartViewComponent { ViewComponentContext = new ViewComponentContext() };
        component.ViewComponentContext.ViewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext();
        component.ViewComponentContext.ViewContext.HttpContext = httpContext;

        // Act
        var result = component.Invoke();

        // Assert
        var viewResult = Assert.IsType<ViewViewComponentResult>(result);
        Assert.Equal(cart, viewResult.ViewData.Model);
    }
}