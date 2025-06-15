using WebLabRest.Models;
using WebLabRest.UI.Services;

namespace WebLabBlazor.Services;

public class ApiProductService : IProductService<Dish>
{
    private readonly HttpClient _http;
    private List<Dish> _dishes;
    private int _currentPage = 1;
    private int _totalPages = 1;

    public IEnumerable<Dish> Products => _dishes;
    public int CurrentPage => _currentPage;
    public int TotalPages => _totalPages;
    public event Action ListChanged;

    public ApiProductService(HttpClient http) => _http = http;

    public async Task GetProducts(int pageNo = 1, int pageSize = 3)
    {
        var response = await _http.GetFromJsonAsync<ResponseData<ListModel<Dish>>>(
            $"/api/Dish?pageNo={pageNo}&pageSize={pageSize}");

        if (response?.Success == true)
        {
            _dishes = response.Data.Items;
            _currentPage = response.Data.CurrentPage;
            _totalPages = response.Data.TotalPages;
            ListChanged?.Invoke();
        }
        else
        {
            _dishes = null!;
            _currentPage = 1;
            _totalPages = 1;
        }
    }
}