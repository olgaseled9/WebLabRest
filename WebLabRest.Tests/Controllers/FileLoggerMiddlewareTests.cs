using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog;
using WebLabRest.UI.Services;
using Xunit;

namespace WebLabRest.Tests.Controllers;

public class FileLoggerMiddlewareTests
{
    [Fact]
    public async Task Invoke_Non2xxResponse_LogsRequest()
    {
        // Arrange
        var logger = Substitute.For<ILogger>();
        Log.Logger = logger;
            
        var next = Substitute.For<RequestDelegate>();
        var context = new DefaultHttpContext();
        context.Response.StatusCode = 404;
            
        var middleware = new FileLogger(next);

        // Act
        await middleware.Invoke(context);

        // Assert
        logger.Received().Information($"---> Request {context.Request.Path} returns {context.Response.StatusCode}");
    }

    [Fact]
    public async Task Invoke_2xxResponse_DoesNotLog()
    {
        // Arrange
        var logger = Substitute.For<ILogger>();
        Log.Logger = logger;
            
        var next = Substitute.For<RequestDelegate>();
        var context = new DefaultHttpContext();
        context.Response.StatusCode = 200;
            
        var middleware = new FileLogger(next);

        // Act
        await middleware.Invoke(context);

        // Assert
        logger.DidNotReceive().Information(Arg.Any<string>());
    }
}
