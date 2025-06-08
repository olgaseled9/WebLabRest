using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLab.Entities;
using WebLabRest.Data;

namespace WebLabRest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly AppDbContext _context;

    public DishesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dish>>> GetDishes(string? category, int pageNo = 1, int pageSize = 3)
    {
        var query = _context.Dishes.Include(d => d.Category).AsQueryable();

        if (!string.IsNullOrEmpty(category))
            query = query.Where(d => d.Category.NormalizedName == category);

        int total = await query.CountAsync();
        int totalPages = (int)Math.Ceiling((double)total / pageSize);

        var items = await query
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            Items = items,
            CurrentPage = pageNo,
            TotalPages = totalPages
        });
    }
}