using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.Study.Controllers;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Tests;

public class SubTopicsControllerTests
{ 
    private readonly Mock<ICrudService<Subject>> _subjectServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock; 
    private readonly SubTopicsController _controller;

    public SubTopicsControllerTests()
    {
        _subjectServiceMock = new Mock<ICrudService<Subject>>();
        _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
        _controller = new SubTopicsController(_subjectServiceMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public void Index_ReturnsViewResult_WithSubjects_When_SubSubjectId_IsNull()
    {
        // Arrange
        var subjects = new List<Subject>
        {
            new Subject { Id = 1, Name = "Subject1", ParentId = null },
            new Subject { Id = 2, Name = "Subject2", ParentId = null } 
        };
        _subjectServiceMock.Setup(s => s.GetAll()).Returns(subjects.AsQueryable());

        // Act
        var result = _controller.Index(null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<Subject>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count); // Ожидаем два Subject'а в результате
    }

    [Fact]
    public void Index_ReturnsViewResult_WithSubSubjects_When_SubSubjectId_IsNotNull()
    {
        // Arrange
        var subjects = new List<Subject>
        {
            new Subject { Id = 1, Name = "SubSubject1", ParentId = 10 },
            new Subject { Id = 2, Name = "SubSubject2", ParentId = 10 }
        };
        _subjectServiceMock.Setup(s => s.GetAll()).Returns(subjects.AsQueryable());

        // Act
        var result = _controller.Index(10);
        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<List<Subject>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count); // Ожидаем два SubSubject'а в результате
        Assert.All(model, s => Assert.Equal(10, s.ParentId));
    }
}