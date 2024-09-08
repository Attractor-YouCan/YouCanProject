using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit.Sdk;
using YouCan.Mvc.Areas.Study.Controllers;
using YouCan.Entities;
using YouCan.Service;
using Microsoft.Extensions.Localization;

namespace YouCan.Tests;

public class LessonsControllerTests
{
    private readonly Mock<ICrudService<Lesson>> _lessonServiceMock;
    private readonly Mock<ICrudService<UserLessons>> _userLessonServiceMock;
    private readonly Mock<ICrudService<UserLevel>> _userLevelServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ICrudService<LessonTime>> _lessonTimeMock;
    private readonly Mock<IStringLocalizer<LessonsController>> _localizerMock;
    private readonly LessonsController _controller;

    public LessonsControllerTests()
    {
        _lessonServiceMock = new Mock<ICrudService<Lesson>>();
        _userLevelServiceMock = new Mock<ICrudService<UserLevel>>();
        _lessonTimeMock = new Mock<ICrudService<LessonTime>>();
        _userLessonServiceMock = new Mock<ICrudService<UserLessons>>();
        _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        _localizerMock = new Mock<IStringLocalizer<LessonsController>>();
        _controller = new LessonsController(_lessonServiceMock.Object, _userLessonServiceMock.Object, _userLevelServiceMock.Object, _lessonTimeMock.Object, _userManagerMock.Object, _localizerMock.Object);
    }

    [Fact]
    public async Task Index_Returns_NotFound_When_No_Lessons()
    {
        // Arrange
        _lessonServiceMock.Setup(s => s.GetAll()).Returns(new List<Lesson>().AsQueryable());

        var user = new User { Id = 1, AvatarUrl = "asda", Disctrict = "asf", EmailConfirmed = true, FullName = "fasasf", UserLessonScore = 1, };          _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);

        _userLessonServiceMock.Setup(s => s.GetAll()).Returns(new List<UserLessons>().AsQueryable());

        // Act
        var result = await _controller.Index(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Empty(viewResult.Model as List<Lesson>); // Проверяем, что модель пуста, если уроков нет.
    }


    [Fact]
    public async Task Index_Returns_ViewResult_With_Lessons()
    {
        // Arrange
        var lessons = new List<Lesson> { new Lesson { Id = 1, SubjectId = 1 } };
        _lessonServiceMock.Setup(s => s.GetAll()).Returns(lessons.AsQueryable());
        var user = new User { Id = 1 };
        _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
        _userLessonServiceMock.Setup(s => s.GetAll()).Returns(new List<UserLessons>().AsQueryable());

        // Act
        var result = await _controller.Index(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Lesson>>(viewResult.Model);
        Assert.Single(model);
    }
      
    [Fact]
    public async Task Details_Returns_NotFound_When_Lesson_Is_Null()
    {
        // Arrange
        _lessonServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync((Lesson)null);

        // Act
        var result = await _controller.Details(1);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No Lesson!", notFoundResult.Value);
    }

    [Fact]
    public async Task Details_Returns_ViewResult_When_Lesson_Is_Valid()
    {
        // Arrange
        var lesson = new Lesson { Id = 1, LessonLevel = 1, SubjectId = 1 };
        _lessonServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(lesson);

        var user = new User { Id = 1, AvatarUrl = "asda", Disctrict = "asf", EmailConfirmed = true, FullName = "fasasf", UserLessonScore = 1 };
        _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);

        var userLessons = new UserLessons { UserId = user.Id, SubjectId = 1, PassedLevel = 1 };
        _userLessonServiceMock.Setup(s => s.GetAll()).Returns(new List<UserLessons> { userLessons }.AsQueryable());

        var userLevel = new UserLevel { UserId = user.Id, SubjectId = lesson.SubjectId, Level = 1 };
        _userLevelServiceMock.Setup(s => s.GetAll()).Returns(new List<UserLevel> { userLevel }.AsQueryable());

        // Act
        var result = await _controller.Details(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Lesson>(viewResult.Model);
        Assert.Equal(lesson.Id, model.Id);
    }

}