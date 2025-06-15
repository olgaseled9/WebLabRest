namespace WebLabRest.Tests;

using Xunit;
using WebLabRest.Models;

public class CategoryTests
{
    [Fact]
    public void Category_InitializesPropertiesCorrectly()
    {
        // Arrange
        var category = new Category
        {
            Id = 1,
            Name = "Desserts",
            NormalizedName = "DESSERTS"
        };

        // Assert
        Assert.Equal(1, category.Id);
        Assert.Equal("Desserts", category.Name);
        Assert.Equal("DESSERTS", category.NormalizedName);
    }

    [Fact]
    public void Category_PropertiesCanBeModified()
    {
        var category = new Category();

        category.Id = 2;
        category.Name = "Soups";
        category.NormalizedName = "SOUPS";

        Assert.Equal(2, category.Id);
        Assert.Equal("Soups", category.Name);
        Assert.Equal("SOUPS", category.NormalizedName);
    }
}
