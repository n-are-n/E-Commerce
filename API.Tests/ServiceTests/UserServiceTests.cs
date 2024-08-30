using Moq;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using API.Models;
using API.Interfaces;
using API.Services;
using API.DTOs;
using Newtonsoft.Json;
namespace API.Tests.ServiceTests;
[TestFixture]
public class UserServiceTests
{
    private Mock<ILogger<UserService>> _logger;
    private Mock<IDatabase> _idatabase;
    private Mock<IConnectionMultiplexer> _redis;
    private Mock<IUserRepository> _repository;
    private UserService? _service;
    [SetUp]
    public void SetUp()
    {
        _logger = new();
        _idatabase = new();
        _redis = new();
        _repository = new();
        _service = new(_logger.Object, _redis.Object, _repository.Object);
    }
    [Test]
    public void GetExceptionTest()
    {
        _redis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Throws(It.IsAny<Exception>());
        Assert.ThrowsAsync<NullReferenceException>(async () => await _service!.Get(It.IsAny<UserDTO>()));
        _redis.Verify(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Service : Get Method")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
    [Test]
    public void GetRedisDataTest()
    {
        UserDTO userDTO = new() { Number = "0123456789", Password = "password" };
        User user = new() { Mail = "mail@domain.com", Name = "name", Number = "0123456789", Password = "password"};
        string userString = JsonConvert.SerializeObject(user);
        _redis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_idatabase.Object);
        _idatabase.Setup(i => i.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(userString);
        var result = _service!.Get(userDTO).Result;
        _idatabase.Verify(i => i.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once);
        _redis.Verify(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Service : Get Method")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<User>());
            Assert.That(result.Name, Is.EqualTo("name"));
            Assert.That(result.Number, Is.EqualTo("0123456789"));
            Assert.That(result.Mail, Is.EqualTo("mail@domain.com"));
            Assert.That(result.Password, Is.EqualTo("password"));
        });
    }
    [Test]
    public void GetDatabaseDataTest()
    {
        UserDTO userDTO = new() { Number = "0123456789", Password = "password" };
        User user = new() { Mail = "mail@domain.com", Name = "name", Number = "0123456789", Password = "password" };
        _redis.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_idatabase.Object);
        _idatabase.Setup(i => i.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(It.IsAny<RedisValue>());
        _repository.Setup(r => r.GetUser(It.IsAny<UserDTO>())).Returns(user);
        var result = _service!.Get(userDTO).Result;
        _repository.Verify(r => r.GetUser(It.IsAny<UserDTO>()), Times.Once);
        _idatabase.Verify(i => i.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once);
        _redis.Verify(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Service : Get Method")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf<User>());
            Assert.That(result.Name, Is.EqualTo("name"));
            Assert.That(result.Number, Is.EqualTo("0123456789"));
            Assert.That(result.Mail, Is.EqualTo("mail@domain.com"));
            Assert.That(result.Password, Is.EqualTo("password"));
        });
    }
    [Test]
    public void PostExceptionTest()
    {
        _repository.Setup(r => r.AddUser(It.IsAny<User>())).ThrowsAsync(It.IsAny<Exception>());
        var result = Assert.ThrowsAsync<ArgumentNullException>(async () => await _service!.Post(It.IsAny<User>()));
        _repository.Verify(r => r.AddUser(It.IsAny<User>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Service : Post Method")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
    [Test]
    public async Task PostNTest()
    {
        _repository.Setup(r => r.AddUser(It.IsAny<User>())).ReturnsAsync(It.IsAny<bool>());
        var result = await _service!.Post(It.IsAny<User>());
        _repository.Verify(r => r.AddUser(It.IsAny<User>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Service : Post Method")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        Assert.That(result, Is.Null);
    }
    [Test]
    public async Task PostTest()
    {
        _repository.Setup(r => r.AddUser(It.IsAny<User>())).ReturnsAsync(true);
        var result = await _service!.Post(It.IsAny<User>());
        _repository.Verify(r => r.AddUser(It.IsAny<User>()), Times.Once);
        _logger.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Service : Post Method")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.TypeOf<string>());
            Assert.That(result, Is.EqualTo("User Added Successfully:-)"));
        });
    }
    [TearDown]
    public void TearDown()
    {
        _logger.Reset();
        _redis.Reset();
        _repository.Reset();
        _service = null;
    }
}