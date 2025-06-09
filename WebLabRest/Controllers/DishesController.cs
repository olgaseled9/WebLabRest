using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLabRest.Data;
using WebLabRest.Models;
using WebLabRest.UI.Services;

namespace WebLabRest.Controllers;


[Route("api/[controller]")]
[ApiController]
public class DishesController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public DishesController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<ActionResult<ResponseData<ListModel<Dish>>>> GetDishes(
        string? category, int pageNo = 1, int pageSize = 3)
    {
        var query = _context.Dishes.AsQueryable();
        
        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(d => d.Category.NormalizedName.Equals(category));
        }
        
        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        
        var items = await query
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .Include(d => d.Category)
            .ToListAsync();
        
        return new ResponseData<ListModel<Dish>>
        {
            Data = new ListModel<Dish>
            {
                Items = items,
                CurrentPage = pageNo,
                TotalPages = totalPages
            }
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseData<Dish>>> GetDish(int id)
    {
        var dish = await _context.Dishes
            .Include(d => d.Category)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (dish == null)
        {
            return NotFound(new ResponseData<Dish>
            {
                Success = false,
                ErrorMessage = "Dish not found"
            });
        }

        return new ResponseData<Dish> { Data = dish };
    }

    [HttpPost]
    public async Task<ActionResult<ResponseData<Dish>>> PostDish(Dish dish)
    {
        _context.Dishes.Add(dish);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetDish", new { id = dish.Id }, 
            new ResponseData<Dish> { Data = dish });
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> SaveImage(int id, IFormFile image)
    {
        var dish = await _context.Dishes.FindAsync(id);
        if (dish == null) return NotFound();
        
        var imagesPath = Path.Combine(_env.WebRootPath, "Images");
        if (!Directory.Exists(imagesPath))
        {
            Directory.CreateDirectory(imagesPath);
        }
        
        var randomName = Path.GetRandomFileName();
        var extension = Path.GetExtension(image.FileName);
        var fileName = Path.ChangeExtension(randomName, extension);
        var filePath = Path.Combine(imagesPath, fileName);
        
        using var stream = System.IO.File.OpenWrite(filePath);
        await image.CopyToAsync(stream);
        
        var host = $"{Request.Scheme}://{Request.Host}";
        var url = $"{host}/Images/{fileName}";
        
        dish.Image = url;
        await _context.SaveChangesAsync();
        
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutDish(int id, Dish dish)
    {
        if (id != dish.Id) return BadRequest();
        
        _context.Entry(dish).State = EntityState.Modified;
        
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DishExists(id)) return NotFound();
            throw;
        }
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDish(int id)
    {
        var dish = await _context.Dishes.FindAsync(id);
        if (dish == null) return NotFound();
        
        _context.Dishes.Remove(dish);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpPost("{id}/upload")]
    public async Task<IActionResult> UploadImage(int id, IFormFile file)
    {
        var dish = await _context.Dishes.FindAsync(id);
        if (dish == null) return NotFound();

        var ext = Path.GetExtension(file.FileName);
        var fileName = Path.GetRandomFileName() + ext;
        var imagePath = Path.Combine(_env.WebRootPath, "Images", fileName);

        using var stream = new FileStream(imagePath, FileMode.Create);
        await file.CopyToAsync(stream);

        dish.Image = $"https://localhost:7002/Images/{fileName}";
        await _context.SaveChangesAsync();

        return Ok(new { dish.Image });
    }
    private bool DishExists(int id)
    {
        return _context.Dishes.Any(e => e.Id == id);
    }
}