using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.Train.Controllers;
using YouCan.Areas.Train.Dto;
using YouCan.Entities;
using YouCan.Service.Service;
using YouCan.Services;
using YouCan.Tests.Services;

namespace YouCan.Tests.Train;

public class QuestionsControllerTests
{
    private readonly Mock<ICRUDService<Question>> _questionCrudServiceMock;
    private readonly Mock<ICRUDService<Subject>> _subjectCrudServiceMock;
    private readonly FakeW3RootFileManager _w3RootFileManagerFake;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly QuestionsController _controller;

    public QuestionsControllerTests()
    {
        _questionCrudServiceMock = new Mock<ICRUDService<Question>>();
        _subjectCrudServiceMock = new Mock<ICRUDService<Subject>>();
        _w3RootFileManagerFake = new FakeW3RootFileManager();
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null
        );  
        _controller = new QuestionsController(
            _questionCrudServiceMock.Object,
            _subjectCrudServiceMock.Object,
            _w3RootFileManagerFake,
            _userManagerMock.Object);
    }
    
    [Fact]
    public async Task CreateAsync_ValidModel_RedirectsToCreateQuestionsWithText_WhenSubjectIsChildAndUserTestTypeIsTest()
    {
        // Arrange
        var subjectId = 1;
        var subject = new Subject { Id = subjectId, SubjectType = SubjectType.Child, UserTestType = Entites.Models.UserTestType.Test };

        _subjectCrudServiceMock.Setup(s => s.GetById(subjectId)).ReturnsAsync(subject);

        // Act
        var result = await _controller.CreateAsync(subjectId);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Create", redirectResult.ActionName);
        Assert.Equal("QuestionsWithText", redirectResult.ControllerName);
        Assert.Equal(subjectId, redirectResult.RouteValues["SubSubjectId"]);
    }

    [Fact]
    public async Task CreateAsync_InvalidModel_ReturnsRedirectToIndex()
    {
        // Act
        var result = await _controller.CreateAsync(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Test", redirectResult.ControllerName);
    }
    
    [Fact]
    public async Task Create_ValidQuestion_RedirectsToIndex()
    {
        // Arrange
        var questionDto = new QuestionDto
        {
            SubjectId = 1,
            Text = "Question text",
            AnswerIsImage = false,
            Answers = new List<AnswerDto>
            {
                new AnswerDto { Text = "Answer 1" },
                new AnswerDto { Text = "Answer 2" },
                new AnswerDto { Text = "Answer 3" },
                new AnswerDto { Text = "Answer 4" }
            },
            CorrectAnswerId = 0
        };
        var subject = new Subject { Id = 1, SubjectType = SubjectType.Child };

        _subjectCrudServiceMock.Setup(s => s.GetById(questionDto.SubjectId)).ReturnsAsync(subject);
        _userManagerMock.Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("1");

        // Act
        var result = await _controller.Create(questionDto);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Test", redirectResult.ControllerName);
    }

    [Fact]
    public async Task Create_InvalidQuestion_ReturnsViewWithModelError()
    {
        // Arrange
        var questionDto = new QuestionDto
        {
            SubjectId = 1,
            Text = "Question text",
            AnswerIsImage = false,
            Answers = new List<AnswerDto>
            {
                new AnswerDto { Text = "Answer 1" },
                new AnswerDto { Text = "Answer 2" }
            },
            CorrectAnswerId = 0
        };
        var subject = new Subject { Id = 1, SubjectType = SubjectType.Child };

        _subjectCrudServiceMock.Setup(s => s.GetById(questionDto.SubjectId)).ReturnsAsync(subject);

        // Act
        var result = await _controller.Create(questionDto);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.Equal(subject, viewResult.ViewData["Subject"]);
    }
    
    [Fact]
    public async Task Created_ValidQuestionId_ReturnsView()
    {
        // Arrange
        var questionId = 1;
        var question = new Question { Id = questionId, UserId = 1, IsPublished = false };

        _questionCrudServiceMock.Setup(s => s.GetById(questionId)).ReturnsAsync(question);
        _userManagerMock.Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns("1");

        // Act
        var result = await _controller.Created(questionId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Created_InvalidQuestionId_ReturnsRedirectToIndex()
    {
        // Arrange
        _questionCrudServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync((Question)null);

        // Act
        var result = await _controller.Created(1);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Test", redirectResult.ControllerName);
    }



}