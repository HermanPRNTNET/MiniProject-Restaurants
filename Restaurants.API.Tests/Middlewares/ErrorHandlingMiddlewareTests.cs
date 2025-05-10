using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using FluentAssertions;

namespace Restaurants.API.Middlewares.Tests;

public class ErrorHandlingMiddlewareTests
{
    [Fact()]
    public async Task InvokeAsync_WhenNoExceptionThrown_ShouldCallNextDelegate()
    {
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleWare = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var nextDelegate = new Mock<RequestDelegate>();

        await middleWare.InvokeAsync(context, nextDelegate.Object); 

        nextDelegate.Verify(n => n.Invoke(context), Times.Once);
    }


    [Fact()]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_ShouldSetStatusCode404()
    {
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleWare = new ErrorHandlingMiddleware(loggerMock.Object);            
        var context = new DefaultHttpContext();
        var notFoundException = new NotFoundException(nameof(Restaurants),"1");

        await middleWare.InvokeAsync(context, _ => throw notFoundException);

        context.Response.StatusCode.Should().Be(404);
    }

    [Fact()]
    public async Task InvokeAsync_WhenForbidenExceptionThrown_ShouldSetStatusCode403()
    {
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleWare = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new ForbidenException();

        await middleWare.InvokeAsync(context, _ => throw exception);

        context.Response.StatusCode.Should().Be(403);
    }
    [Fact()]
    public async Task InvokeAsync_WhenInternalServerError_ShouldSetStatusCode500()
    {
        var loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
        var middleWare = new ErrorHandlingMiddleware(loggerMock.Object);
        var context = new DefaultHttpContext();
        var exception = new SystemException();

        await middleWare.InvokeAsync(context, _ => throw exception);

        context.Response.StatusCode.Should().Be(500);
    }
}