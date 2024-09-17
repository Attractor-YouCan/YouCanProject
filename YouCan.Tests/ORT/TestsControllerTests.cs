using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using YouCan.Areas.ORT.Controllers;
using YouCan.Areas.Study.ViewModels;
using YouCan.Entities;
using YouCan.Service.Service;
using YouCan.ViewModels;
using YouCan.ViewModels.Account;

namespace YouCan.Tests.ORT
{
    public class TestsControllerTests
    {
        private readonly Mock<ICRUDService<OrtTest>> _mockOrtTestService;
        private readonly Mock<ICRUDService<UserOrtTest>> _mockUserOrtTestService;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IImpactModeService> _mockImpactModeService;
        private readonly TestsController _controller;

        public TestsControllerTests()
        {
            _mockOrtTestService = new Mock<ICRUDService<OrtTest>>();
            _mockUserOrtTestService = new Mock<ICRUDService<UserOrtTest>>();
            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);
            _mockImpactModeService = new Mock<IImpactModeService>();
            _controller = new TestsController(
                _mockOrtTestService.Object,
                _mockUserOrtTestService.Object,
                _mockUserManager.Object,
                _mockImpactModeService.Object
                );
        }

        [Fact]
        public async Task Index_Get_ReturnsViewResult_WithCorrectModel()
        {
            // Arrange
            var ortTestId = 1;
            var ortTest = new OrtTest { Id = ortTestId, OrtLevel = 1, Tests = new List<Test>() };
            var user = new User { Id = 1 };
            var userOrtTest = new UserOrtTest { UserId = user.Id, PassedLevel = 1 };

            _mockOrtTestService.Setup(s => s.GetById(ortTestId)).ReturnsAsync(ortTest);
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mockUserOrtTestService.Setup(s => s.GetById(user.Id)).ReturnsAsync(userOrtTest);

            // Act
            var result = await _controller.Index(ortTestId);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(ortTest.Tests, viewResult.Model);
        }

        [Fact]
        public async Task Index_Get_RedirectsToAction_WhenUserIsNotAuthenticated()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync((User)null);

            // Act
            var result = await _controller.Index(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("no ort test", notFoundResult.Value);
        }

        [Fact]
        public async Task Index_Post_ReturnsBadRequest_WhenOrtTestIsNull()
        {
            // Arrange
            var testSubmissionModel = new TestSubmissionModel { OrtTestId = 1 };
            _mockOrtTestService.Setup(s => s.GetById(testSubmissionModel.OrtTestId)).ReturnsAsync((OrtTest)null);

            // Act
            var result = await _controller.Index(testSubmissionModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No OrtTest Id!", badRequestResult.Value);
        }

        [Fact]
        public async Task Index_Post_ReturnsOkResult_WithOrtTestResultModels()
        {
            // Arrange
            var ortTestId = 1;
            var ortTest = new OrtTest
            {
                Id = ortTestId,
                OrtLevel = 1,
                Tests = new List<Test>
            {
                new Test
                {
                    Id = 1,
                    Questions = new List<Question>
                    {
                        new Question
                        {
                            Id = 1,
                            Answers = new List<Answer>
                            {
                                new Answer { Id = 1, IsCorrect = true }
                            },
                            Point = 10
                        }
                    }
                }
            }
            };
            var user = new User { Id = 1 };
            var userOrtTest = new UserOrtTest { UserId = user.Id, PassedLevel = 1 };
            var testSubmissionModel = new TestSubmissionModel
            {
                OrtTestId = ortTestId,
                SelectedAnswers = new List<TestAnswersModel>
                {
                    new TestAnswersModel { AnswerId = 1, QuestionId = 1 }
                },
                TimeSpent = new List<OrtTestModel>
                {
                    new OrtTestModel { TestId = 1, TimeSpent = 600 }
                }
            };

            _mockOrtTestService.Setup(s => s.GetById(ortTestId)).ReturnsAsync(ortTest);
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mockUserOrtTestService.Setup(s => s.GetById(user.Id)).ReturnsAsync(userOrtTest);

            // Act
            var result = await _controller.Index(testSubmissionModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultData = Assert.IsType<List<OrtTestResultModel>>(okResult.Value.GetType().GetProperty("ortTestResultModels").GetValue(okResult.Value));
            Assert.NotEmpty(resultData);
        }
    }
}
