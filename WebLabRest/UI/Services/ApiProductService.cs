using System.Text.Json;
using WebLabRest.Models;

namespace WebLabRest.UI.Services;

public class ApiProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ApiProductService> _logger;

    public ApiProductService(HttpClient httpClient, ILogger<ApiProductService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ResponseData<ListModel<Dish>>> GetProductListAsync(
        string? categoryNormalizedName, int pageNo = 1)
    {
        var url = $"api/dishes?pageNo={pageNo}"
            + (string.IsNullOrEmpty(categoryNormalizedName) ? "" : $"&category={categoryNormalizedName}");
        
        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Dish>>>();
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Ошибка: {ex.Message}");
                return new ResponseData<ListModel<Dish>>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка: {ex.Message}"
                };
            }
        }

        _logger.LogError($"Не удалось получить продукты: {response.StatusCode}");
        return new ResponseData<ListModel<Dish>>
        {
            Success = false,
            ErrorMessage = $"Не удалось получить продукты: {response.StatusCode}"
        };
    }

    public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"api/dishes/{id}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ResponseData<Dish>>();
        }

        return new ResponseData<Dish>
        {
            Success = false,
            ErrorMessage = $"Не удалось получить продукт: {response.StatusCode}"
        };
    }

    public async Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile)
    {
        var responseData = new ResponseData<Dish>();
        var response = await _httpClient.PostAsJsonAsync("api/dishes", product);
        
        if (!response.IsSuccessStatusCode)
        {
            responseData.Success = false;
            responseData.ErrorMessage = $"Ошибка: {response.StatusCode}";
            return responseData;
        }
        
        if (formFile != null)
        {
            var dish = await response.Content.ReadFromJsonAsync<Dish>();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{_httpClient.BaseAddress}api/dishes/{dish.Id}")
            };
            
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "image", formFile.FileName);
            request.Content = content;
            
            response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"Ошибка изображения: {response.StatusCode}";
            }
        }
        
        return responseData;
    }

    public async Task UpdateProductAsync(Dish product)
    {
        await _httpClient.PutAsJsonAsync($"api/dishes/{product.Id}", product);
    }

    public async Task DeleteProductAsync(int id)
    {
        await _httpClient.DeleteAsync($"api/dishes/{id}");
    }
}