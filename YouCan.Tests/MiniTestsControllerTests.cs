using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.Study.Controllers;
using YouCan.Areas.Study.ViewModels;
using YouCan.Entities;
using YouCan.Service.Service;
using YouCan.Tests.Services;

namespace YouCan.Tests;

public class MiniTestsControllerTests
{

    private readonly Mock<ICRUDService<Lesson>> _lessonServiceMock;
    private readonly Mock<ICRUDService<UserLessons>> _userLessonServiceMock;
    private readonly Mock<ICRUDService<Test>> _testServiceMock;
    private readonly Mock<ICRUDService<UserLevel>> _userLevelServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly MiniTestsController _controller;

    public MiniTestsControllerTests()
    {
        _lessonServiceMock = new Mock<ICRUDService<Lesson>>();
        _userLessonServiceMock = new Mock<ICRUDService<UserLessons>>();
        _testServiceMock = new Mock<ICRUDService<Test>>();
        _userLevelServiceMock = new Mock<ICRUDService<UserLevel>>();
        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

        _controller = new MiniTestsController(
            _lessonServiceMock.Object,
            _userLessonServiceMock.Object,
            _testServiceMock.Object,
            _userLevelServiceMock.Object,
            _userManagerMock.Object
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
        var userLesson = new UserLessons { UserId = 1, SubjectId = 1, PassedLevel = 0 };

        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
            .ReturnsAsync(currentUser);
        _testServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(test);
        _lessonServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(lesson);
        _userLessonServiceMock.Setup(s => s.GetAll()).Returns(new List<UserLessons> { userLesson }.AsQueryable());

        var selectedAnswers = new List<TestAnswersModel>
        {
            new TestAnswersModel { TestId = 1, AnswerId = 1 }
        };

        // Act
        var result = await _controller.Index(selectedAnswers);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultData = Assert.IsType<TestResult>(okResult.Value);
        Assert.True(resultData.IsPassed);
        Assert.Equal(lesson.Id, resultData.LessonId);
        Assert.Equal(lesson.SubjectId, resultData.SubtopicId);
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