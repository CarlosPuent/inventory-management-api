using InventoryApi.Exceptions;
using InventoryApi.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Xunit;

namespace InventoryApi.Tests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionHandlingMiddleware>> _loggerMock;

    public ExceptionHandlingMiddlewareTests()
    {
        _loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();
    }

    private async Task<(int StatusCode, string Body)> InvokeMiddlewareAsync(Exception exceptionToThrow)
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        RequestDelegate next = _ => throw exceptionToThrow;

        var middleware = new ExceptionHandlingMiddleware(next, _loggerMock.Object);
        await middleware.InvokeAsync(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(context.Response.Body).ReadToEndAsync();

        return (context.Response.StatusCode, body);
    }

    [Fact]
    public async Task InvokeAsync_WhenBusinessRuleExceptionThrown_Returns400()
    {
        var (statusCode, body) = await InvokeMiddlewareAsync(new BusinessRuleException("Precio invalido."));

        Assert.Equal(400, statusCode);
        Assert.Contains("Precio invalido.", body);
    }

    [Fact]
    public async Task InvokeAsync_WhenConflictExceptionThrown_Returns409()
    {
        var (statusCode, _) = await InvokeMiddlewareAsync(new ConflictException("Conflicto de datos."));

        Assert.Equal(409, statusCode);
    }

    [Fact]
    public async Task InvokeAsync_WhenUnknownExceptionThrown_Returns500WithGenericMessage()
    {
        var (statusCode, body) = await InvokeMiddlewareAsync(new InvalidOperationException("Detalle interno sensible"));

        Assert.Equal(500, statusCode);
        Assert.DoesNotContain("Detalle interno sensible", body);
    }
}