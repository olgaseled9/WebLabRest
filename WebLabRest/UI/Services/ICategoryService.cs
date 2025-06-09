using WebLabRest.Models;

namespace WebLabRest.UI.Services;

public interface ICategoryService
{
    Task<ResponseData<List<Category>>> GetCategoryListAsync();
}