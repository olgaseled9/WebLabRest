namespace WebLabRest.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
}