using WebLabRest.Models;

namespace WebLabRest.UI.Services;

public class ApiCategoryService(HttpClient httpClient) : ICategoryService
{
    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var response = await httpClient.GetAsync(httpClient.BaseAddress);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<ResponseData<List<Category>>>();
            return result ?? new ResponseData<List<Category>> { Success = false, ErrorMessage = "Пустой ответ" };
        }

        return new ResponseData<List<Category>>
        {
            Success = false,
            ErrorMessage = "Ошибка при запросе категорий"
        };
    }
}