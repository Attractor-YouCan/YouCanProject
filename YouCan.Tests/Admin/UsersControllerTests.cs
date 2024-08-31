using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using YouCan.Areas.Admin.Controllers;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Mvc;
using YouCan.Repository;
using YouCan.Service.Service;

namespace YouCan.Tests.Admin;

public class UsersControllerTests
{
    private readonly Mock<IUserStore<User>> _userStoreMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IWebHostEnvironment> _envMock;
    private readonly Mock<ICRUDService<AdminAction>> _adminActionsMock;

    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        // Настройка мока IUserStore
        _userStoreMock = new Mock<IUserStore<User>>();
        
        // Настройка мока UserManager
        _userManagerMock = new Mock<UserManager<User>>(
            _userStoreMock.Object,
            null, null, null, null, null, null, null, null);
        _envMock = new Mock<IWebHostEnvironment>();
        _adminActionsMock = new Mock<ICRUDService<AdminAction>>();

        // Setting up the controller
        _controller = new UsersController(null, _userManagerMock.Object, _envMock.Object, _adminActionsMock.Object);
    }
    
    [Fact]
    public async Task ChangeRole_ValidRole_ChangesUserRole()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "user1" };
        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "user" });
        _userManagerMock.Setup(u => u.RemoveFromRolesAsync(user, It.IsAny<IEnumerable<string>>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.AddToRoleAsync(user, "admin")).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User { Id = 2, UserName = "admin" });

        // Act
        var result = await _controller.ChangeRole(1, "admin");

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        _userManagerMock.Verify(u => u.AddToRoleAsync(user, "admin"), Times.Once);
    }

    
    [Fact]
    public async Task Block_User_BlocksUser()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "user1" };
        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.SetLockoutEnabledAsync(user, true)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.SetLockoutEndDateAsync(user, It.IsAny<DateTimeOffset>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User { Id = 2, UserName = "admin" });

        // Act
        var result = await _controller.Block(1);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        _userManagerMock.Verify(u => u.SetLockoutEnabledAsync(user, true), Times.Once);
    }

    [Fact]
    public async Task Unblock_User_UnblocksUser()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "user1" };
        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.SetLockoutEnabledAsync(user, false)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.SetLockoutEndDateAsync(user, null)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User { Id = 2, UserName = "admin" });

        // Act
        var result = await _controller.Unblock(1);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        _userManagerMock.Verify(u => u.SetLockoutEnabledAsync(user, false), Times.Once);
    }

    
    [Fact]
    public async Task Edit_ValidUser_RedirectsToIndex()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "user1", Email = "test@test.com" };
        _userManagerMock.Setup(u => u.FindByIdAsync("1")).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User { Id = 2, UserName = "admin" });

        // Act
        var result = await _controller.Edit(user, 1, null, null);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
        _userManagerMock.Verify(u => u.UpdateAsync(user), Times.Once);
    }
    
}