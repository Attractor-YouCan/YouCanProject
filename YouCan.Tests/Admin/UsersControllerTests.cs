using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.Admin.Controllers;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Tests.Admin;

public class UsersControllerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<RoleManager<IdentityRole<int>>> _roleManagerMock;
    private readonly Mock<ICRUDService<AdminAction>> _adminActionsMock;
    private readonly Mock<ICRUDService<Tariff>> _tariffManagerMock;
    private readonly Mock<IWebHostEnvironment> _envMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        var roleStoreMock = new Mock<IRoleStore<IdentityRole<int>>>();
        _roleManagerMock = new Mock<RoleManager<IdentityRole<int>>>(roleStoreMock.Object, null, null, null, null);
        _adminActionsMock = new Mock<ICRUDService<AdminAction>>();
        _tariffManagerMock = new Mock<ICRUDService<Tariff>>();
        _envMock = new Mock<IWebHostEnvironment>();

        _controller = new UsersController(_userManagerMock.Object, _envMock.Object, _adminActionsMock.Object, _roleManagerMock.Object, _tariffManagerMock.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfUsers()
    {
        // Arrange
        var users = new List<User> { new User { Id = 1, UserName = "TestUser" } };
        _userManagerMock.Setup(m => m.Users).Returns(users.AsQueryable());
        _tariffManagerMock.Setup(m => m.GetAll()).Returns(new List<Tariff>().AsQueryable());

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<User>>(viewResult.ViewData.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task ChangeRole_ValidRole_ChangesUserRole()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "TestUser" };
        var admin = new User { Id = 2, UserName = "AdminUser" }; 

        _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "user" });
        _userManagerMock.Setup(m => m.AddToRoleAsync(user, "admin")).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(admin); 

        // Act
        var result = await _controller.ChangeRole(1, "manager");

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public async Task ChangeTariff_ValidTariff_ChangesUserTariff()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "TestUser",  Tariff = new Tariff { Name = "Basic" }, TariffId = 1};
        var tariff = new Tariff { Id = 2, Name = "Premium", Duration = 6 };
        var admin = new User { Id = 2, UserName = "AdminUser" };

        _userManagerMock.Setup(m => m.Users).Returns(new List<User> { user }.AsQueryable());
        _tariffManagerMock.Setup(m => m.GetById(It.IsAny<int>())).ReturnsAsync(tariff);
        _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(admin);

        // Act
        var result = await _controller.ChangeTariff(1, 2);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
        _userManagerMock.Verify(m => m.UpdateAsync(user), Times.Once);
    }

    [Fact]
    public async Task Block_ValidUser_BlocksUser()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "TestUser" };
        var admin = new User { Id = 2, UserName = "AdminUser" };
        _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.SetLockoutEnabledAsync(user, true)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.SetLockoutEndDateAsync(user, DateTime.MaxValue.ToUniversalTime())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(admin);


        // Act
        var result = await _controller.Block(1);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public async Task Unblock_ValidUser_UnblocksUser()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "TestUser" };
        var admin = new User { Id = 2, UserName = "AdminUser" };
        _userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.SetLockoutEndDateAsync(user, null)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.SetLockoutEnabledAsync(user, false)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(m => m.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(admin);

        // Act
        var result = await _controller.Unblock(1);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }
}