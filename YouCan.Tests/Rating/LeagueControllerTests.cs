using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using YouCan.Mvc.Areas.Rating.Controllers;
using YouCan.Entities;
using YouCan.Service;

namespace YouCan.Tests.Rating;

public class LeagueControllerTests
{
    private readonly Mock<ICrudService<League>> _leagueServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly LeagueController _controller;

    public LeagueControllerTests()
    {
        _leagueServiceMock = new Mock<ICrudService<League>>();
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _controller = new LeagueController(_leagueServiceMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task Index_Returns_ViewResult_With_Leagues()
    {
        // Arrange
        var leagues = new List<League>
        {
            new League { Id = 1, LeagueName = "League 1" },
            new League { Id = 2, LeagueName = "League 2" }
        }.AsQueryable();

        _leagueServiceMock.Setup(s => s.GetAll()).Returns(leagues);

        // Act
        var result = await _controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<League>>(viewResult.Model);
        Assert.Equal(2, model.Count);
    }

    [Fact]
    public async Task GetUsersByLeague_Returns_JsonResult_With_Users()
    {
        // Arrange
        var league = new League { Id = 1, LeagueName = "League 1", MinPoints = 100, MaxPoints = 500 };
        var leagues = new List<League> { league }.AsQueryable();

        var users = new List<User>
        {
            new User { Id = 1, FullName = "User 1", UserLessonScore = 150, Rank = 1, League = league },
            new User { Id = 2, FullName = "User 2", UserLessonScore = 300, Rank = 2, League = league }
        }.AsQueryable();

        _leagueServiceMock.Setup(s => s.GetAll()).Returns(leagues);
        _userManagerMock.Setup(u => u.Users).Returns(users);

        // Expected anonymous objects
        var expectedUsers = new List<object>
        {
            new { Id = 2, UserName = (string)null!, FullName = "User 2", AvatarUrl = (string)null!, Rank = 2, UserLessonScore = 300 },
            new { Id = 1, UserName = (string)null!, FullName = "User 1", AvatarUrl = (string)null!, Rank = 1, UserLessonScore = 150 }
        };

        // Act
        var result = await _controller.GetUsersByLeague("League 1");

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);

        // Serialize expected and actual values to compare
        var expectedJson = JsonConvert.SerializeObject(expectedUsers);
        var actualJson = JsonConvert.SerializeObject(jsonResult.Value);

        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public async Task Create_Valid_League_Returns_RedirectToAction()
    {
        // Arrange
        var league = new League { LeagueName = "New League", MinPoints = 100, MaxPoints = 500 };

        // Act
        var result = await _controller.Create(league);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }

    [Fact]
    public async Task Edit_Existing_League_Returns_ViewResult()
    {
        // Arrange
        var league = new League { Id = 1, LeagueName = "League 1", MinPoints = 100, MaxPoints = 500 };

        _leagueServiceMock.Setup(s => s.GetById(1)).ReturnsAsync(league);

        // Act
        var result = await _controller.Edit(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<League>(viewResult.Model);
        Assert.Equal(league.Id, model.Id);
    }

    [Fact]
    public async Task DeleteConfirmed_ValidId_Deletes_League()
    {
        // Arrange
        var league = new League { Id = 1, LeagueName = "League 1" };

        _leagueServiceMock.Setup(s => s.GetById(1)).ReturnsAsync(league);
        _leagueServiceMock.Setup(s => s.DeleteById(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteConfirmed(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
    }
}