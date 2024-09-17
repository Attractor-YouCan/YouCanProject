using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.Train.Controllers;
using YouCan.Areas.Train.Dto;
using YouCan.Entities;
using YouCan.Service.Service;


namespace YouCan.Tests.Train;

public class QuestionsWithTextControllerTests
{
    private readonly Mock<ICRUDService<Subject>> _subjectCrudServiceMock;
    private readonly Mock<ICRUDService<Test>> _testCrudServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly QuestionsWithTextController _controller;

    public QuestionsWithTextControllerTests()
    {
        _subjectCrudServiceMock = new Mock<ICRUDService<Subject>>();
        _testCrudServiceMock = new Mock<ICRUDService<Test>>();
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null
        );
        _controller = new QuestionsWithTextController(
            _testCrudServiceMock.Object,
            _subjectCrudServiceMock.Object,
            null, 
            _userManagerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ValidSubject_ReturnsViewOrRedirect()
    {
        // Arrange
        var subjectId = 1;
        var subject = new Subject
        {
            Id = subjectId,
            SubjectType = SubjectType.Child,
            UserTestType = Entites.Models.UserTestType.Question
        };

        _subjectCrudServiceMock.Setup(s => s.GetById(subjectId)).ReturnsAsync(subject);

        // Act
        var result = await _controller.CreateAsync(subjectId);

        // Assert
        if (subject.UserTestType == Entites.Models.UserTestType.Question)
        {
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Create", redirectResult.ActionName);
            Assert.Equal("QuestionsWithText", redirectResult.ControllerName);
            Assert.Equal(subjectId, redirectResult.RouteValues["SubSubjectId"]);
        }
        else
        {
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["Subject"]);
        }
    }

    [Fact]
    public async Task Create_InvalidSubject_ReturnsRedirectToIndex()
    {
        // Arrange
        _subjectCrudServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync((Subject)null);

        // Act
        var result = await _controller.CreateAsync(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Test", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Create_ValidTest_RedirectsToIndex()
    {
        // Arrange
        var testDto = new TestDto
        {
            SubjectId = 1,
            Text = "Test text",
            Questions = new List<TestDto.TestQuestionDto>
            {
                new TestDto.TestQuestionDto
                {
                    Text = "Question 1",
                    Answers = new List<string> { "Answer 1", "Answer 2", "Answer 3", "Answer 4" },
                    CorrectAnswerId = 0
                }
            }
        };

        var subject = new Subject { Id = 1, SubjectType = SubjectType.Child, UserTestType = Entites.Models.UserTestType.Test };
        _subjectCrudServiceMock.Setup(s => s.GetById(testDto.SubjectId)).ReturnsAsync(subject);
        _userManagerMock.Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("1");

        // Act
        var result = await _controller.Create(testDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Test", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Create_InvalidTest_ReturnsViewWithModelError()
    {
        // Arrange
        var testDto = new TestDto
        {
            SubjectId = 1,
            Text = "Test text",
            Questions = new List<TestDto.TestQuestionDto>
            {
                new TestDto.TestQuestionDto
                {
                    Text = "Question 1",
                    Answers = new List<string> { "Answer 1", "Answer 2" }, // Менее 4 ответов
                    CorrectAnswerId = 0
                }
            }
        };

        var subject = new Subject { Id = 1, SubjectType = SubjectType.Child, UserTestType = Entites.Models.UserTestType.Test };
        _subjectCrudServiceMock.Setup(s => s.GetById(testDto.SubjectId)).ReturnsAsync(subject);

        // Act
        var result = await _controller.Create(testDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.Equal(subject, viewResult.ViewData["Subject"]);
    }

    [Fact]
    public async Task Created_ValidTest_ReturnsView()
    {
        // Arrange
        var testId = 1;
        var test = new Test { Id = testId, UserId = 1, IsPublished = false };

        _testCrudServiceMock.Setup(t => t.GetById(testId)).ReturnsAsync(test);
        _userManagerMock.Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("1");

        // Act
        var result = await _controller.Created(testId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Created_InvalidTest_ReturnsRedirectToIndex()
    {
        // Arrange
        _testCrudServiceMock.Setup(t => t.GetById(It.IsAny<int>())).ReturnsAsync((Test)null);

        // Act
        var result = await _controller.Created(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Test", redirectResult.ControllerName);
    }
}
