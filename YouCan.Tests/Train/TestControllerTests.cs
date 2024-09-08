using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.Train.Controllers;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Tests;

public class TestControllerTests
{
     private readonly TestController _controller;
     private readonly Mock<ICrudService<Test>> _testServiceMock;

     public TestControllerTests()
     {
         _testServiceMock = new Mock<ICrudService<Test>>();
         _controller = new TestController(_testServiceMock.Object);
     }

     [Fact]
     public async Task Index_ReturnsViewResult_WithListOfTests()
     {
         // Arrange
         int subSubjectId = 1;
         var tests = new List<Test>
         {
             new Test { Id = 1, SubjectId = subSubjectId, LessonId = null },
             new Test { Id = 2, SubjectId = subSubjectId, LessonId = null }
         };
         
         _testServiceMock.Setup(s => s.GetAll()).Returns(tests.AsQueryable());

         // Act
         var result = await _controller.Index(subSubjectId);

         // Assert
         var viewResult = Assert.IsType<ViewResult>(result);
         var model = Assert.IsAssignableFrom<List<Test>>(viewResult.Model);
         Assert.Equal(2, model.Count);
         Assert.All(model, t => Assert.Equal(subSubjectId, t.SubjectId));
     }

     [Fact]
     public async Task Index_ReturnsViewResult_WithEmptyListWhenNoTestsFound()
     {
         // Arrange
         int subSubjectId = 1;
         var tests = new List<Test>(); // Empty list

         _testServiceMock.Setup(s => s.GetAll()).Returns(tests.AsQueryable());

         // Act
         var result = await _controller.Index(subSubjectId);

         // Assert
         var viewResult = Assert.IsType<ViewResult>(result);
         var model = Assert.IsAssignableFrom<List<Test>>(viewResult.Model);
         Assert.Empty(model); // Ensure the list is empty
     }

     [Fact]
     public async Task Index_ReturnsViewResult_WithFilteredTests()
     {
         // Arrange
         int subSubjectId = 1;
         var tests = new List<Test>
         {
             new Test { Id = 1, SubjectId = subSubjectId, LessonId = null },
             new Test { Id = 2, SubjectId = subSubjectId + 1, LessonId = null } // Different SubjectId
         };

         _testServiceMock.Setup(s => s.GetAll()).Returns(tests.AsQueryable());
                
         // Act
         var result = await _controller.Index(subSubjectId);

         // Assert
         var viewResult = Assert.IsType<ViewResult>(result);
         var model = Assert.IsAssignableFrom<List<Test>>(viewResult.Model);
         Assert.Single(model); // Only one test with the correct SubjectId
         Assert.Equal(subSubjectId, model.First().SubjectId);
     }
}