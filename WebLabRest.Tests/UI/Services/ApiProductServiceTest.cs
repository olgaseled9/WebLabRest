using JetBrains.Annotations;
using WebLabRest.UI.Services;

namespace WebLabRest.Tests.UI.Services;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using WebLabRest.Models;
using WebLabRest.UI.Services;
using Xunit;

public class ApiProductServiceTests
{
    private readonly Mock<ILogger<ApiProductService>> _loggerMock = new();

    private HttpClient GetHttpClientMock(HttpResponseMessage response)
    {
        var handlerMock = new Mock<HttpMessageHandler>();

        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(), 
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com/")
        };
    }

    [Fact]
    public async Task GetProductListAsync_ReturnsData_WhenSuccess()
    {
        var responseData = new ResponseData<ListModel<Dish>>
        {
            Success = true,
            Data = new ListModel<Dish>
            {
                Items = new List<Dish>
                {
                    new Dish { Id = 1, Name = "Pizza" }
                },
                CurrentPage = 1,
                TotalPages = 1
            }
        };

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(responseData)
        };

        var client = GetHttpClientMock(response);
        var service = new ApiProductService(client, _loggerMock.Object);

        var result = await service.GetProductListAsync(null);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data.Items);
        Assert.Equal("Pizza", result.Data.Items[0].Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsDish_WhenSuccess()
    {
        var dishResponse = new ResponseData<Dish>
        {
            Success = true,
            Data = new Dish { Id = 1, Name = "Burger" }
        };

        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = JsonContent.Create(dishResponse)
        };

        var client = GetHttpClientMock(response);
        var service = new ApiProductService(client, _loggerMock.Object);

        var result = await service.GetProductByIdAsync(1);

        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Burger", result.Data.Name);
    }

    [Fact]
    public async Task GetProductByIdAsync_ReturnsError_WhenResponseNotSuccessful()
    {
        var response = new HttpResponseMessage(HttpStatusCode.NotFound);

        var client = GetHttpClientMock(response);
        var service = new ApiProductService(client, _loggerMock.Object);

        var result = await service.GetProductByIdAsync(999);

        Assert.False(result.Success);
        Assert.Contains("Не удалось получить продукт", result.ErrorMessage);
    }
}
