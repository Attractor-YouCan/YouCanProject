using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.Admin.Controllers;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Tests.Admin;

public class StatControllerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ICRUDService<Test>> _testServiceMock;
    private readonly Mock<ICRUDService<Tariff>> _tariffServiceMock;
    private readonly StatController _controller;

    public StatControllerTests()
    {
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null
        );
        _testServiceMock = new Mock<ICRUDService<Test>>();
        _tariffServiceMock = new Mock<ICRUDService<Tariff>>();

        _controller = new StatController(_userManagerMock.Object, _testServiceMock.Object, _tariffServiceMock.Object);
    }

    [Fact]
    public async Task Stat_Returns_ViewResult_With_Correct_Data()
    {
        // Arrange
        var tests = new List<Test>
        {
            new Test { Id = 1, Subject = new Subject { Name = "Math" }},
            new Test { Id = 2, Subject = new Subject { Name = "Math" }},
            new Test { Id = 3, Subject = null }
        }.AsQueryable();

        var users = new List<User>
        {
            new User { Id = 1, Tariff = new Tariff { Name = "Pro" }, CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new User { Id = 2, Tariff = new Tariff { Name = "Start" }, CreatedAt = DateTime.UtcNow.AddDays(-20) },
            new User { Id = 3, Tariff = null, CreatedAt = DateTime.UtcNow.AddDays(-40) }
        }.AsQueryable();

        _testServiceMock.Setup(s => s.GetAll()).Returns(tests);
        _userManagerMock.Setup(u => u.Users).Returns(users);

        _tariffServiceMock.Setup(t => t.GetAll())
            .Returns(new List<Tariff> {
                new Tariff { Name = "Start" },
                new Tariff { Name = "Pro" },
                new Tariff { Name = "Premium" }
            }.AsQueryable());

        // Act
        var result = await _controller.Stat();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.ViewData["Tests"]);
        Assert.NotNull(viewResult.ViewData["Users"]);
        Assert.Equal(3, viewResult.ViewData["TestCount"]);
        Assert.Equal(3, viewResult.ViewData["UsersCount"]);
    }

    [Fact]
    public void CalculateAllNewUsers_Returns_Correct_Percentage()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new User { Id = 2, CreatedAt = DateTime.UtcNow.AddDays(-20) },
            new User { Id = 3, CreatedAt = DateTime.UtcNow.AddDays(-40) }
        }.AsQueryable();

        _userManagerMock.Setup(u => u.Users).Returns(users);

        // Act
        var result = _controller.CalculateAllNewUsers();

        // Assert
        Assert.Equal(66.67, result, 2);
    }

    [Fact]
    public void CalculateStartUsers_Returns_Correct_Percentage()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Tariff = new Tariff { Name = "Start" }, TariffStartDate = DateTime.UtcNow.AddDays(-10) },
            new User { Id = 2, Tariff = new Tariff { Name = "Pro" }, TariffStartDate = DateTime.UtcNow.AddDays(-20) },
            new User { Id = 3, Tariff = new Tariff { Name = "Start" }, TariffStartDate = DateTime.UtcNow.AddDays(-20) }
        }.AsQueryable();

        _userManagerMock.Setup(u => u.Users).Returns(users);

        // Act
        var result = _controller.CalcualteStartUsers();

        // Assert
        Assert.Equal(66.67, result, 2);
    }

    [Fact]
    public void CalculateProUsers_Returns_Correct_Percentage()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Tariff = new Tariff { Name = "Pro" }, TariffStartDate = DateTime.UtcNow.AddDays(-10) },
            new User { Id = 2, Tariff = new Tariff { Name = "Pro" }, TariffStartDate = DateTime.UtcNow.AddDays(-5) },
            new User { Id = 3, Tariff = new Tariff { Name = "Start" }, TariffStartDate = DateTime.UtcNow.AddDays(-20) }
        }.AsQueryable();

        _userManagerMock.Setup(u => u.Users).Returns(users);

        // Act
        var result = _controller.CalculateProUsers();

        // Assert
        Assert.Equal(66.67, result, 2);
    }

    [Fact]
    public void CalculatePremiumUsers_Returns_Correct_Percentage()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Tariff = new Tariff { Name = "Premium" }, TariffStartDate = DateTime.UtcNow.AddDays(-10) },
            new User { Id = 2, Tariff = new Tariff { Name = "Premium" }, TariffStartDate = DateTime.UtcNow.AddDays(-50) },
            new User { Id = 3, Tariff = new Tariff { Name = "Pro" }, TariffStartDate = DateTime.UtcNow.AddDays(-40) }
        }.AsQueryable();

        _userManagerMock.Setup(u => u.Users).Returns(users);

        // Act
        var result = _controller.CalculatePremiumUsers();

        // Assert
        Assert.Equal(66.67, result, 2);
    }
}