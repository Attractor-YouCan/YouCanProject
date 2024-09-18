using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Mvc.Areas.Study.Controllers;
using YouCan.Entities;
using YouCan.Service;
using YouCan.Mvc.Areas.Ort.ViewModels;

namespace YouCan.Tests;

public class MiniTestsControllerTests
{

    private readonly Mock<ICRUDService<Lesson>> _lessonServiceMock;
    private readonly Mock<ICRUDService<UserLessons>> _userLessonServiceMock;
    private readonly Mock<ICRUDService<Test>> _testServiceMock;
    private readonly Mock<ICRUDService<UserLevel>> _userLevelServiceMock;
    private readonly Mock<IImpactModeService> _impactModeServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly MiniTestsController _controller;

    public MiniTestsControllerTests()
    {
        _lessonServiceMock = new Mock<ICRUDService<Lesson>>();
        _userLessonServiceMock = new Mock<ICRUDService<UserLessons>>();
        _testServiceMock = new Mock<ICRUDService<Test>>();
        _userLevelServiceMock = new Mock<ICRUDService<UserLevel>>();
        _impactModeServiceMock = new Mock<IImpactModeService>();
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        _controller = new MiniTestsController(
            _lessonServiceMock.Object,
            _userLessonServiceMock.Object,
            _testServiceMock.Object,
            _userLevelServiceMock.Object,
            _userManagerMock.Object,
            _impactModeServiceMock.Object
        );
    }

    [Fact]
    public void Index_Get_ReturnsNotFound_WhenNoTest()
    {
        // Arrange
        _testServiceMock.Setup(s => s.GetAll())
            .Returns(new List<Test>().AsQueryable());

        // Act
        var result = _controller.Index(1).Result;

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No Test!", notFoundResult.Value);
    }

    [Fact]
    public void Index_Get_ReturnsViewResult_WithTest()
    {
        // Arrange
        var test = new Test { LessonId = 1 };
        _testServiceMock.Setup(s => s.GetAll())
            .Returns(new List<Test> { test }.AsQueryable());

        // Act
        var result = _controller.Index(1).Result;

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(test, viewResult.Model);
    }

    [Fact]
    public async Task Index_Post_ReturnsOkResult_WithTestResult()
    {
        // Arrange
        var currentUser = new User { Id = 1 };
        var lesson = new Lesson { Id = 1, LessonLevel = 1, SubjectId = 1 };
        var test = new Test
        {
            Id = 1,
            LessonId = 1,
            Questions = new List<Question>
            {
                new Question
                {
                    Id = 1,
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, IsCorrect = true },
                        new Answer { Id = 2, IsCorrect = false }
                    }
                }
            }
        };
        var userLesson = new UserLessons { UserId = 1, SubjectId = 1, PassedLevel = 0, IsPassed = true, LessonId = 1};
        var userLevel = new UserLevel() { UserId = 1, SubjectId = 1, Level = 0 };
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
            .ReturnsAsync(currentUser);
        _testServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(test);
        _lessonServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(lesson);
        _userLessonServiceMock.Setup(s => s.GetAll()).Returns(new List<UserLessons> { userLesson }.AsQueryable());
        _userLevelServiceMock.Setup(s => s.GetAll()).Returns(new List<UserLevel> {userLevel});
        var selectedAnswers = new List<TestAnswersModel>
        {
            new TestAnswersModel { TestId = 1, AnswerId = 1 }
        };

        // Act
        var result = await _controller.Index(selectedAnswers);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultData = okResult.Value;

        Assert.Equal(true, resultData.GetType().GetProperty("isPassed")?.GetValue(resultData));
        Assert.Equal(lesson.Id, resultData.GetType().GetProperty("lessonId")?.GetValue(resultData));
        Assert.Equal((int?)lesson.SubjectId, resultData.GetType().GetProperty("subtopicId")?.GetValue(resultData));
    }

    [Fact]
    public void Result_ReturnsViewResult_WithCorrectViewData()
    {
        // Arrange
        bool isPassed = true;
        int lessonId = 1;
        int subtopicId = 2;

        // Act
        var result = _controller.Result(isPassed, lessonId, subtopicId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(lessonId, viewResult.ViewData["LessonId"]);
        Assert.Equal(subtopicId, viewResult.ViewData["SubtopicId"]);
        Assert.Equal(isPassed, viewResult.Model);
    }

}
