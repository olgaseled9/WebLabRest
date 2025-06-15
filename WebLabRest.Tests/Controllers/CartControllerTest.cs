using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebLabRest.Controllers;
using WebLabRest.Models;
using WebLabRest.UI.Services;
using Xunit;


namespace WebLabRest.UI.Tests
{
    public class CartControllerTests
    {
        private readonly CartController _controller;
        private readonly IProductService _productService;

        public CartControllerTests()
        {
            _productService = Substitute.For<IProductService>();
            _controller = new CartController(_productService);
            _controller.ControllerContext = new ControllerContext();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        }

        [Fact]
        public async Task Add_ValidId_AddsToCart()
        {
            // Arrange
            var dish = new Dish { Id = 1 };
            _productService.GetProductByIdAsync(1).Returns(new ResponseData<Dish> { Data = dish });

            // Act
            var result = await _controller.Add(1, "/");

            // Assert
            var cart = _controller.HttpContext.Session.Get<Cart>("cart");
            Assert.NotNull(cart);
            Assert.Equal(1, cart.CartItems.Count);
        }

        [Fact]
        public void Remove_ExistingItem_RemovesFromCart()
        {
            // Arrange
            var cart = new Cart();
            cart.AddToCart(new Dish { Id = 1 });
            _controller.HttpContext.Session.Set("cart", cart);

            // Act
            var result = _controller.Remove(1);

            // Assert
            var updatedCart = _controller.HttpContext.Session.Get<Cart>("cart");
            Assert.Empty(updatedCart.CartItems);
        }

        [Fact]
        public void Index_EmptyCart_ReturnsEmptyView()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Empty(viewResult.Model as Dictionary<int, CartItem>);
        }
    }
}