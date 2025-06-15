using JetBrains.Annotations;
using WebLabRest.Controllers;

namespace WebLabRest.Tests.Controllers;

using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using WebLabRest.Controllers;
using System.Text.Json;

public class SessionExtensionsTests
{
    [Fact]
    public void Set_SerializesAndStoresValue()
    {
        // Arrange
        var mockSession = new Mock<ISession>();
        byte[] storedBytes = null!;
        
        mockSession
            .Setup(s => s.SetString(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((key, value) => storedBytes = System.Text.Encoding.UTF8.GetBytes(value));
        
        var testObject = new TestData { Id = 42, Name = "Test" };

        // Act
        mockSession.Object.Set("test_key", testObject);

        // Assert
        Assert.NotNull(storedBytes);
        var json = System.Text.Encoding.UTF8.GetString(storedBytes);
        var deserialized = JsonSerializer.Deserialize<TestData>(json);
        Assert.NotNull(deserialized);
        Assert.Equal(testObject.Id, deserialized.Id);
        Assert.Equal(testObject.Name, deserialized.Name);
    }

    [Fact]
    public void Get_ReturnsDeserializedObject_WhenValueExists()
    {
        // Arrange
        var testObject = new TestData { Id = 42, Name = "Test" };
        var json = JsonSerializer.Serialize(testObject);

        var mockSession = new Mock<ISession>();
        mockSession.Setup(s => s.GetString("test_key")).Returns(json);

        // Act
        var result = mockSession.Object.Get<TestData>("test_key");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testObject.Id, result.Id);
        Assert.Equal(testObject.Name, result.Name);
    }

    [Fact]
    public void Get_ReturnsDefault_WhenValueIsNull()
    {
        // Arrange
        var mockSession = new Mock<ISession>();
        mockSession.Setup(s => s.GetString("test_key")).Returns((string?)null);

        // Act
        var result = mockSession.Object.Get<TestData>("test_key");

        // Assert
        Assert.Null(result);  // default for reference type is null
    }

    private class TestData
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
