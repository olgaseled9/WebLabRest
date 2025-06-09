using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLabRest.Models;
using WebLabRest.UI.Services;

namespace WebLabRest.UI.Areas.Admin.Pages.Dishes;

[Authorize(Policy = "admin")]
public class IndexModel : PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    public List<Dish> Dish { get; set; } = default!;
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;

    public async Task OnGetAsync(int? pageNo = 1)
    {
        var response = await _productService.GetProductListAsync(null, pageNo.Value);
        if (response.Success)
        {
            Dish = response.Data.Items;
            CurrentPage = response.Data.CurrentPage;
            TotalPages = response.Data.TotalPages;
        }
    }

    public async Task OnGetAsync(int pageNo = 1)
    {
        var result = await _productService.GetProductListAsync(null, pageNo);
        if (result.Success)
        {
            Dish = result.Data.Items;
            CurrentPage = result.Data.CurrentPage;
            TotalPages = result.Data.TotalPages;
        }
    }
}