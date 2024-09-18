using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using YouCan.Mvc.Areas.Admin.Controllers;
using YouCan.Mvc.Areas.Admin.ViewModels;
using YouCan.Entities;
using YouCan.Service;

namespace YouCan.Tests.Admin;

public class StudyControllerTests
{
    private readonly Mock<ICrudService<Subject>> _mockSubjectService;
    private readonly Mock<ICrudService<Lesson>> _mockLessonService;
    private readonly Mock<ICrudService<LessonModule>> _mockLessonModuleService;
    private readonly Mock<ICrudService<Test>> _mockTestService;
    private readonly Mock<ICrudService<Question>> _mockQuestionService;
    private readonly Mock<ICrudService<Answer>> _mockAnswerService;
    private readonly Mock<IWebHostEnvironment> _mockEnvironment;
    private readonly Mock<ICRUDService<AdminAction>> _mockAdminActions;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly StudyController _controller;

    public StudyControllerTests()
    {
        _mockSubjectService = new Mock<ICrudService<Subject>>();
        _mockLessonService = new Mock<ICrudService<Lesson>>();
        _mockLessonModuleService = new Mock<ICrudService<LessonModule>>();
        _mockTestService = new Mock<ICrudService<Test>>();
        _mockQuestionService = new Mock<ICrudService<Question>>();
        _mockAnswerService = new Mock<ICrudService<Answer>>();
        _mockEnvironment = new Mock<IWebHostEnvironment>();
        _mockUserManager = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null
        );
        _mockAdminActions = new Mock<ICRUDService<AdminAction>>();

        _controller = new StudyController(
            _mockSubjectService.Object,
            _mockLessonService.Object,
            _mockLessonModuleService.Object,
            _mockTestService.Object,
            _mockQuestionService.Object,
            _mockAnswerService.Object,
            _mockEnvironment.Object,
            _mockUserManager.Object,
            _mockAdminActions.Object
        );
    }
    
    [Fact]
    public async Task Index_Returns_ViewResult_With_Subjects()
    {
        // Arrange
        var subjects = new List<Subject> { new Subject { Id = 1, Name = "Test Subject" } };
        _mockSubjectService.Setup(s => s.GetAll())
            .Returns(subjects.AsQueryable());

        // Act
        var result = await _controller.Index(null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Subject>>(viewResult.Model);
        Assert.Single(model);
        Assert.Equal("Test Subject", model.First().Name);
    }

    [Fact]
    public async Task AllLessons_Returns_ViewResult_With_Lessons()
    {
        // Arrange
        var lessons = new List<Lesson> { new Lesson { Id = 1, Title = "Lesson 1", SubjectId = 1 } };
        _mockLessonService.Setup(s => s.GetAll())
            .Returns(lessons.AsQueryable());

        // Act
        var result = await _controller.AllLessons(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Lesson>>(viewResult.Model);
        Assert.Single(model);
        Assert.Equal("Lesson 1", model.First().Title);
    }

    [Fact]
    public async Task CreateLesson_Returns_ViewResult_With_Correct_SubjectId()
    {
        // Arrange
        var subject = new Subject { Id = 1, Name = "Test Subject", Lessons = new List<Lesson>()};
        _mockSubjectService.Setup(s => s.GetById(1))
            .ReturnsAsync(subject);

        // Act
        var result = await _controller.CreateLesson(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(1, viewResult.ViewData["SubjectId"]);
    }

    [Fact]
    public async Task CreateLesson_Post_Returns_OkResult_When_Valid()
    {
        var user = new User { Id = 1 };
        _mockUserManager.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
        // Arrange
        var lessonModel = new LessonModel
        {
            SubjectId = 1,
            LessonTitle = "New Lesson",
            LessonLevel = 1,
            Modules = new List<LessonModuleModel>(),
            Questions = new List<QuestionModel>()
        };

        // Act
        var result = await _controller.CreateLesson(lessonModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);

        var json = JsonConvert.SerializeObject(okResult.Value); 
        var value = JsonConvert.DeserializeObject<Dictionary<string, object>>(json); 

        Assert.True(value.ContainsKey("subjectId")); 
        Assert.Equal(1, Convert.ToInt32(value["subjectId"])); 
    }
    
    [Fact]
    public async Task Edit_Returns_ViewResult_With_Correct_LessonModel()
    {
        // Arrange
        var lesson = new Lesson
        {
            Id = 1,
            Title = "Lesson 1",
            SubTitle = "Subtitle 1",
            Description = "Description",
            LessonModules = new List<LessonModule>(),
            Tests = new List<Test> { new Test { Id = 1 } }
        };

        _mockLessonService.Setup(s => s.GetById(1))
            .ReturnsAsync(lesson);

        // Act
        var result = await _controller.Edit(1);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LessonModel>(viewResult.Model);
        Assert.Equal("Lesson 1", model.LessonTitle);
    }
    
    [Fact]
    public async Task Edit_Post_Returns_OkResult_When_Valid()
    {
        var user = new User { Id = 1 };
        _mockUserManager.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
        // Arrange
        var lessonModel = new LessonModel
        {
            Id = 1,
            SubjectId = 1,
            LessonTitle = "Updated Lesson",
            Modules = new List<LessonModuleModel>(),
            Questions = new List<QuestionModel>()
        };

        var lesson = new Lesson
        {
            Id = 1,
            SubjectId = 1,
            LessonModules = new List<LessonModule>(),
            Tests = new List<Test> { new Test { Id = 1 } }
        };

        _mockLessonService.Setup(s => s.GetById(1))
            .ReturnsAsync(lesson);

        _mockTestService.Setup(s => s.GetById(It.IsAny<int>()))
            .ReturnsAsync(new Test { Id = 1, Questions = new List<Question>() });

        // Act
        var result = await _controller.Edit(lessonModel);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var jsonResult = JsonConvert.SerializeObject(okResult.Value);
        var dynamicResult = JsonConvert.DeserializeObject<dynamic>(jsonResult);

        Assert.NotNull(dynamicResult);
        Assert.Equal(1, (int)dynamicResult.subjectId);
    }



}