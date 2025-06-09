using System.ComponentModel.DataAnnotations;

namespace WebLabRest.Models;

public class Dish
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    [Range(0, 10000, ErrorMessage = "Calories must be between 0 and 10000")]
    public int Calories { get; set; }
    
    public string? Image { get; set; }
    
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}