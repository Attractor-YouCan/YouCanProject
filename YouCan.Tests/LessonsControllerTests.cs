using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using YouCan.Areas.Study.Controllers;
using YouCan.Entities;
using YouCan.Service.Service;

namespace YouCan.Tests;

public class LessonsControllerTests
{
      private readonly Mock<ICRUDService<Lesson>> _lessonServiceMock;
      private readonly Mock<ICRUDService<UserLessons>> _userLessonServiceMock;
      private readonly Mock<UserManager<User>> _userManagerMock;
      private readonly LessonsController _controller;

      public LessonsControllerTests()
      {
          _lessonServiceMock = new Mock<ICRUDService<Lesson>>();
          _userLessonServiceMock = new Mock<ICRUDService<UserLessons>>();
          _userManagerMock = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);

          _controller = new LessonsController(_lessonServiceMock.Object, _userLessonServiceMock.Object, _userManagerMock.Object);
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

          var user = new User { Id = 1, AvatarUrl = "asda", Disctrict = "asf", EmailConfirmed = true, FullName = "fasasf", UserLessonScore = 1, };          _userManagerMock.Setup(u => u.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);

          var userLessons = new UserLessons { UserId = user.Id, SubjectId = 1, PassedLevel = 1 };
          _userLessonServiceMock.Setup(s => s.GetAll()).Returns(new List<UserLessons> { userLessons }.AsQueryable());

          // Act
          var result = await _controller.Details(1);

          // Assert
          var viewResult = Assert.IsType<ViewResult>(result);
          var model = Assert.IsType<Lesson>(viewResult.Model);
          Assert.Equal(lesson.Id, model.Id);
      }

}