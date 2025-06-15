using System.Collections.Generic;
using System.Threading;
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
        private readonly HttpContext _httpContext;

        public CartControllerTests()
        {
            _productService = Substitute.For<IProductService>();
            _controller = new CartController(_productService);
            
            // Фикс: Полная настройка HttpContext
            _httpContext = new DefaultHttpContext();
            _httpContext.Session = new FakeSession();
            _controller.ControllerContext = new ControllerContext { HttpContext = _httpContext };
        }

        [Fact]
        public async Task Add_ValidId_AddsToCart()
        {
            // Arrange
            var dish = new Dish { Id = 1 };
            _productService.GetProductByIdAsync(1).Returns(new ResponseData<Dish> { Data = dish, Success = true });

            // Act
            var result = await _controller.Add(1, "/");

            // Assert
            var cart = _httpContext.Session.Get<Cart>("cart");
            Assert.NotNull(cart);
            Assert.Equal(1, cart.CartItems.Count);
        }
    }

    public class FakeSession : ISession
    {
        private readonly Dictionary<string, byte[]> _storage = new();
        
        public string Id => "TestSession";
        public bool IsAvailable => true;
        public IEnumerable<string> Keys => _storage.Keys;

        public void Clear() => _storage.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _storage.Remove(key);
        
        public void Set(string key, byte[] value) => _storage[key] = value;
        
        public bool TryGetValue(string key, out byte[] value) => _storage.TryGetValue(key, out value);
    }
}