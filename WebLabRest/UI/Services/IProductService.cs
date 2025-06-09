
using WebLabRest.Models;

namespace WebLabRest.UI.Services;

public interface IProductService
{
    Task<ResponseData<ListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
    Task<ResponseData<Dish>> GetProductByIdAsync(int id);
    Task<ResponseData<Dish>> CreateProductAsync(Dish product, IFormFile? formFile);
    Task UpdateProductAsync(Dish product);
    Task DeleteProductAsync(int id);
}

public class ResponseData<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string? ErrorMessage { get; set; }
}

public class ListModel<T>
{
    public List<T> Items { get; set; } = new();
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}