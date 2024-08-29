using NUnit.Framework.Internal;
using Moq;
using Microsoft.Extensions.Logging;
using API.Controllers;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        _service.Setup(x => x.Post(It.IsAny<User>())).ReturnsAsync(It.IsAny<string>());
        var result = await _controller!.SignUp(It.IsAny<User>());
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
        _service.Setup(x => x.Post(It.IsAny<User>())).ReturnsAsync("Success");
        var result = await _controller!.SignUp(It.IsAny<User>());
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
        _service.Setup(x => x.Post(It.IsAny<User>())).ThrowsAsync(It.IsAny<Exception>());
        var result = await _controller!.SignUp(It.IsAny<User>());
        var objectResult = result as ObjectResult;
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<ObjectResult>());
            Assert.That(objectResult!.StatusCode, Is.EqualTo(406));
            Assert.That(objectResult!.Value, Is.EqualTo("User is not accepted"));
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