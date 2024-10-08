using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Mvc.Areas.Train.Controllers;
using YouCan.Mvc.Areas.Train.ViewModels;
using YouCan.Entities;
using YouCan.Service;

namespace YouCan.Tests;

public class TrainTestControllerTests
{
    private readonly Mock<ICrudService<PassedQuestion>> _passedQuestionServiceMock;
    private readonly Mock<ICrudService<Subject>> _subjectServiceMock;
    private readonly Mock<ICrudService<Test>> _testServiceMock;
    private readonly Mock<ICrudService<Question>> _questionServiceMock;
    private readonly Mock<ICrudService<QuestionReport>> _questionReportServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IImpactModeService> _impactModeServiceMock;
    private readonly TrainTestController _controller;

    public TrainTestControllerTests()
    {
        _passedQuestionServiceMock = new Mock<ICrudService<PassedQuestion>>();
        _subjectServiceMock = new Mock<ICrudService<Subject>>();
        _testServiceMock = new Mock<ICrudService<Test>>();
        _questionServiceMock = new Mock<ICrudService<Question>>();
        _questionReportServiceMock = new Mock<ICrudService<QuestionReport>>();
        _impactModeServiceMock = new Mock<IImpactModeService>();
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null
        );

        _controller = new TrainTestController(
            _passedQuestionServiceMock.Object,
            _subjectServiceMock.Object,
            _questionReportServiceMock.Object,
            _testServiceMock.Object,
            _questionServiceMock.Object,
            _userManagerMock.Object,
            _impactModeServiceMock.Object
        );
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithTestViewModel()
    {
        // Arrange
        var subSubjectId = 1;
        var user = new User { Id = 1 };
        var question = new Question
        {
            Id = 1,
            SubjectId = subSubjectId,
            IsPublished = true
        };
        var subject = new Subject { Id = subSubjectId, Name = "Test Subject" };
        var passedQuestions = new List<PassedQuestion>
        {
            new PassedQuestion { UserId = user.Id, QuestionId = 2 } // вопрос, который пользователь уже ответил
        };

        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
        _questionServiceMock.Setup(qs => qs.GetAll()).Returns(new List<Question> { question }.AsQueryable());
        _subjectServiceMock.Setup(ss => ss.GetAll()).Returns(new List<Subject> { subject }.AsQueryable());
        _passedQuestionServiceMock.Setup(pqs => pqs.GetAll()).Returns(new List<PassedQuestion>().AsQueryable());

        // Act
        var result = await _controller.Index(subSubjectId);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<TestViewModel>(viewResult.Model);
        Assert.Equal(subSubjectId, model.SubjectId);
        Assert.Equal("Test Subject", model.SubjectName);
        Assert.NotNull(model.CurrentQuestion);
        Assert.IsType<PageViewModel>(model.PageViewModel);
    }

    [Fact]
    public async Task GetNextQuestion_ReturnsPartialViewResult_WithQuestion()
    {
        // Arrange
        int currentPage = 1;
        int subtopicId = 1;
        var user = new User { Id = 1 };
        var test = new Test
        {
            SubjectId = subtopicId,
            Questions = new List<Question>
            {
                new Question { Id = 1 },
                new Question { Id = 2 }
            }
        };

        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
        _testServiceMock.Setup(ts => ts.GetAll()).Returns(new List<Test> { test }.AsQueryable());
        _passedQuestionServiceMock.Setup(pqs => pqs.GetAll()).Returns(new List<PassedQuestion>().AsQueryable());

        // Act
        var result = await _controller.GetNextQuestion(currentPage, subtopicId);

        // Assert
        var partialViewResult = Assert.IsType<PartialViewResult>(result);
        Assert.NotNull(partialViewResult.Model);
    }

    [Fact]
    public async Task FinishTest_InsertsPassedQuestions_ReturnsOkResult()
    {
        // Arrange
        var user = new User { Id = 1 };
        var questionId = 1;
        var model = new FinishTestModel
        {
            Answers = new List<AnswerCheckModel>
            {
                new AnswerCheckModel { QuestionId = questionId }
            }
        };

        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
        _questionServiceMock.Setup(qs => qs.GetById(questionId)).ReturnsAsync(new Question { Id = questionId });

        // Act
        var result = await _controller.FinishTest(model);

        // Assert
        Assert.IsType<OkResult>(result);
        _passedQuestionServiceMock.Verify(pqs => pqs.Insert(It.Is<PassedQuestion>(pq => pq.QuestionId == questionId && pq.UserId == user.Id)), Times.Once);
    }

    [Fact]
    public async Task ReportQuestion_InsertsQuestionReport_ReturnsOkResult()
    {
        // Arrange
        var user = new User { Id = 1 };
        var questionId = 1;
        var model = new QuestionReportModel
        {
            QuestionId = questionId,
            Text = "Report text"
        };

        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
        _questionServiceMock.Setup(qs => qs.GetById(questionId)).ReturnsAsync(new Question { Id = questionId });

        // Act
        var result = await _controller.ReportQuestion(model);

        // Assert
        Assert.IsType<OkResult>(result);
        _questionReportServiceMock.Verify(qrs => qrs.Insert(It.Is<QuestionReport>(qr => qr.QuestionId == questionId && qr.UserId == user.Id && qr.Text == model.Text)), Times.Once);
    }


}