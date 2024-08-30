using NUnit.Framework.Internal;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API.Controllers;
using API.Interfaces;
using API.Models;
using API.DTOs;
namespace API.Tests.ControllerTests;
[TestFixture]
public class AuthenticationControllerTests
{
    private AuthenticationController? _controller;
    private Mock<ILogger<AuthenticationController>> _logger;
    private Mock<IUserService> _service;
    [SetUp]
    public void Setup()
    {
        _logger = new();
        _service = new();
        _controller = new(_logger.Object, _service.Object);
    }
    [Test]
    public async Task SignUp400Test()
    {
        _service.Setup(S => S.Post(It.IsAny<User>())).ReturnsAsync(It.IsAny<string>());
        var result = await _controller!.SignUp(It.IsAny<User>());
        _service.Verify(s => s.Post(It.IsAny<User>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Authentication Controller : SignUp Action")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        var badResult = result as BadRequestObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(badResult!.StatusCode, Is.EqualTo(400));
        });
    }
    [Test]
    public async Task SignUp201Test()
    {
        _service.Setup(S => S.Post(It.IsAny<User>())).ReturnsAsync("Success");
        var result = await _controller!.SignUp(It.IsAny<User>());
        _service.Verify(s => s.Post(It.IsAny<User>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Authentication Controller : SignUp Action")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        var createdResult = result as CreatedResult;
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<CreatedResult>());
            Assert.That(createdResult!.StatusCode, Is.EqualTo(201));
        });
    }
    [Test]
    public async Task SignUp406Test()
    {
        _service.Setup(S => S.Post(It.IsAny<User>())).ThrowsAsync(It.IsAny<Exception>());
        var result = await _controller!.SignUp(It.IsAny<User>());
        _service.Verify(s => s.Post(It.IsAny<User>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Authentication Controller : SignUp Action")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        var objectResult = result as ObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<ObjectResult>());
            Assert.That(objectResult!.StatusCode, Is.EqualTo(406));
            Assert.That(objectResult!.Value, Is.EqualTo("User is not accepted"));
        });
    }
    [Test]
    public async Task SignIn401Test()
    {
        _service.Setup(s => s.Get(It.IsAny<UserDTO>())).ThrowsAsync(It.IsAny<Exception>());
        var result = await _controller!.SignIn(It.IsAny<UserDTO>());
        _service.Verify(s => s.Get(It.IsAny<UserDTO>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Authentication Controller : SignIn Action")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        var objectResult = result as ObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<ObjectResult>());
            Assert.That(objectResult!.StatusCode, Is.EqualTo(401));
            Assert.That(objectResult!.Value, Is.EqualTo("User is Unauthorized to access"));
        });
    }
    [Test]
    public async Task SignIn202Test()
    {
        _service.Setup(S => S.Get(It.IsAny<UserDTO>())).ReturnsAsync(It.IsAny<User>());
        var result = await _controller!.SignIn(It.IsAny<UserDTO>());
        _service.Verify(s => s.Get(It.IsAny<UserDTO>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Authentication Controller : SignIn Action")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        var objectResult = result as AcceptedResult;
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<AcceptedResult>());
            Assert.That(objectResult!.StatusCode, Is.EqualTo(202));
        });
    }
    [TearDown]
    public void TearDown()
    {
        _logger.Reset();
        _service.Reset();
        _controller = null;
    }
}