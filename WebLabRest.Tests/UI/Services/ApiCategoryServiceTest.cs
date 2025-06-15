using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using WebLabRest.Models;
using WebLabRest.UI.Services;
using Xunit;
using System.Collections.Generic;

public class ApiCategoryServiceTests
{
    [Fact]
    public async Task GetCategoryListAsync_ReturnsCategories_WhenSuccess()
    {
        // Arrange
        var expectedCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Drinks", NormalizedName = "DRINKS" },
            new Category { Id = 2, Name = "Desserts", NormalizedName = "DESSERTS" }
        };
        var responseData = new ResponseData<List<Category>>
        {
            Success = true,
            Data = expectedCategories
        };

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(responseData)
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new System.Uri("http://test.com/")
        };

        var service = new ApiCategoryService(httpClient);

        // Act
        var result = await service.GetCategoryListAsync();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal("Drinks", result.Data[0].Name);
    }

    [Fact]
    public async Task GetCategoryListAsync_ReturnsError_WhenResponseFails()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
            });

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new System.Uri("http://test.com/")
        };

        var service = new ApiCategoryService(httpClient);

        // Act
        var result = await service.GetCategoryListAsync();

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Ошибка при запросе категорий", result.ErrorMessage);
    }
  
}
