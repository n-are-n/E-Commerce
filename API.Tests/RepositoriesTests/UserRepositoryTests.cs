using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using API.Models;
using API.DTOs;
using API.Repositories;
using API.Data;
namespace API.Tests.RepositoriesTests;
[TestFixture]
public class UserRepositoryTests
{
    private Mock<ILogger<UserRepository>>? _logger;
    private EcommerceContext _context;
    private UserRepository? _repository;
    [SetUp]
    public void SetUp()
    {
        _logger = new();
        var options = new DbContextOptionsBuilder<EcommerceContext>().UseInMemoryDatabase(databaseName: "Ecommerce").Options;
        _context = new(options);
        _repository = new(_logger.Object, _context);
    }
    [Test]
    public void GetUserExceptionTest()
    {
        UserDTO userDTO = new() { Number = "0123456789", Password = "password" };
        Assert.Throws<AggregateException>(() => _repository!.GetUser(userDTO));
        _logger!.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Repository : Get User Function")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
    [Test]
    public async Task GetUserTest()
    {
        UserDTO userDTO = new() { Number = "0123456789", Password = "password" };
        User user = new() { Mail = "mail@domain.com", Name = "name", Number = "0123456789", Password = "password" };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        User result = _repository!.GetUser(userDTO);
        _logger!.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Repository : Get User Function")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
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
    public async Task AddUserExceptionTest()
    {
        User user = new() { Mail = "mail@domain.com", Name = "name", Number = "0123456789", Password = "password" };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        Assert.ThrowsAsync<ArgumentException>(async () => await _repository!.AddUser(user));
        _logger!.Verify(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Repository : Add User Function")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
    [Test]
    public async Task AddUserTest()
    {
        User user = new() { Mail = "mail@domain.com", Name = "name", Number = "0123456789", Password = "password" };
        bool result = await _repository!.AddUser(user);
        Assert.That(result, Is.True);
        _logger!.Verify(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("User Repository : Add User Function")), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
    [TearDown]
    public void TearDown()
    {
        _logger = null;
        _repository = null;
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}