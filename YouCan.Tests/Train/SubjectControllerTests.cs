using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.Train.Controllers;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Tests;

public class SubjectControllerTests
{
     private readonly SubjectController _controller;
     private readonly Mock<ICrudService<Subject>> _subjectServiceMock;
     private readonly Mock<UserManager<User>> _userManagerMock;

     public SubjectControllerTests()
     {
         _subjectServiceMock = new Mock<ICrudService<Subject>>();
         _userManagerMock = new Mock<UserManager<User>>(
             new Mock<IUserStore<User>>().Object,
             null, null, null, null, null, null, null, null);
         _controller = new SubjectController(_subjectServiceMock.Object, _userManagerMock.Object);
     }

     [Fact]
     public void Index_ReturnsViewResult_WithListOfSubjects_WhenSubSubjectIdIsNull()
     {
         // Arrange
         var subjects = new List<Subject>
         {
             new Subject { Id = 1, ParentId = null },
             new Subject { Id = 2, ParentId = null }
         };

         _subjectServiceMock.Setup(s => s.GetAll()).Returns(subjects.AsQueryable());

         // Act
         var result = _controller.Index(null);

         // Assert
         var viewResult = Assert.IsType<ViewResult>(result);
         var model = Assert.IsAssignableFrom<List<Subject>>(viewResult.Model);
         Assert.Equal(2, model.Count);
         Assert.All(model, s => Assert.Null(s.ParentId)); // Ensure ParentId is null
     }

     [Fact]
     public void Index_ReturnsViewResult_WithFilteredSubjects_WhenSubSubjectIdIsProvided()
     {
         // Arrange
         int subSubjectId = 1;
         var subjects = new List<Subject>
         {
             new Subject { Id = 1, ParentId = subSubjectId },
             new Subject { Id = 2, ParentId = subSubjectId },
             new Subject { Id = 3, ParentId = subSubjectId + 1 } // Different ParentId
         };

         _subjectServiceMock.Setup(s => s.GetAll()).Returns(subjects.AsQueryable());

         // Act
         var result = _controller.Index(subSubjectId);

         // Assert
         var viewResult = Assert.IsType<ViewResult>(result);
         var model = Assert.IsAssignableFrom<List<Subject>>(viewResult.Model);
         Assert.Equal(2, model.Count);
         Assert.All(model, s => Assert.Equal(subSubjectId, s.ParentId)); // Ensure ParentId matches
     }

     [Fact] 
     public void Index_SetsViewBag_ForCreateFlag()
     {
           // Arrange
           int subSubjectId = 1;
           var subjects = new List<Subject>();
         
           _subjectServiceMock.Setup(s => s.GetAll()).Returns(subjects.AsQueryable());

           // Act
           var result = _controller.Index(subSubjectId, true);

           // Assert
           var viewResult = Assert.IsType<ViewResult>(result);
           Assert.True(viewResult.ViewData["ForCreate"] is bool forCreateFlag && forCreateFlag);
     }
}