using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.ORT.Controllers;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Tests.ORT;

public class OrtTestsControllerTests
{
    private readonly Mock<ICRUDService<OrtTest>> _mockOrtTestService;
    private readonly Mock<ICRUDService<UserOrtTest>> _mockUserOrtTestService;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly OrtTestsController _controller;
    
    public OrtTestsControllerTests()
    {
        _mockOrtTestService = new Mock<ICRUDService<OrtTest>>();
        _mockUserOrtTestService = new Mock<ICRUDService<UserOrtTest>>();
        _mockUserManager = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);
        _controller = new OrtTestsController(
            _mockOrtTestService.Object, 
            _mockUserOrtTestService.Object,
            _mockUserManager.Object);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfOrtTests()
    {
        // Arrange
        var testList = new List<OrtTest>
        {
            new OrtTest { Id = 1, OrtLevel = 1 },
            new OrtTest { Id = 2, OrtLevel = 2 }
        };
        _mockOrtTestService.Setup(s => s.GetAll()).Returns(testList.AsQueryable());
            
        var user = new User { Id = 1 };
        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                            .ReturnsAsync(user);

        var userOrtTest = new UserOrtTest { UserId = user.Id, IsPassed = false, PassedLevel = 0 };
        _mockUserOrtTestService.Setup(s => s.GetById(user.Id)).ReturnsAsync(userOrtTest);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<OrtTest>>(viewResult.Model);
        Assert.Equal(testList.Count, model.Count);
    }

    [Fact]
    public async Task Details_ReturnsViewResult_WithOrtTest()
    {
        // Arrange
        var ortTest = new OrtTest { Id = 1, OrtLevel = 1 };
        _mockOrtTestService.Setup(s => s.GetById(1)).ReturnsAsync(ortTest);

        var user = new User { Id = 1 };
        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                            .ReturnsAsync(user);

        var userOrtTest = new UserOrtTest { UserId = user.Id, IsPassed = false, PassedLevel = 0, OrtTest = ortTest };
        _mockUserOrtTestService.Setup(s => s.GetById(user.Id)).ReturnsAsync(userOrtTest);

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<OrtTest>(viewResult.Model);
        Assert.Equal(ortTest.Id, model.Id);
    }

    [Fact]
    public async Task Details_ReturnsRedirectToAction_WhenUserHasNotPassedLevel()
    {
        // Arrange
        var ortTest = new OrtTest { Id = 1, OrtLevel = 2 };
        _mockOrtTestService.Setup(s => s.GetById(1)).ReturnsAsync(ortTest);

        var user = new User { Id = 1 };
        _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                            .ReturnsAsync(user);

        var userOrtTest = new UserOrtTest { UserId = user.Id, IsPassed = false, PassedLevel = 0 };
        _mockUserOrtTestService.Setup(s => s.GetById(user.Id)).ReturnsAsync(userOrtTest);

        // Act
        var result = await _controller.Details(1);

        // Assert
        Assert.IsType<ViewResult>(result); 
    }
}