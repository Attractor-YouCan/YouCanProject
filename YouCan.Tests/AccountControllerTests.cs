using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using YouCan.Entites.Models;
using YouCan.Entities;
using YouCan.Mvc;
using YouCan.Service.Service;
using YouCan.ViewModels;

public class AccountControllerTests
{
    private readonly Mock<IUserCRUD> _userServiceMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly Mock<IWebHostEnvironment> _environmentMock;
    private readonly Mock<ICRUDService<UserLevel>> _userLevelMock;
    private readonly Mock<ICRUDService<UserLessons>> _userLessonServiceMock;
    private readonly Mock<ICRUDService<Tariff>> _tariffsMock;
    private readonly TwoFactorService _twoFactorService;  
    private readonly AccountController _controller;
    private readonly Mock<ICRUDService<UserExperience>> _userExperienceMock;

    public AccountControllerTests()
    {
        _userServiceMock = new Mock<IUserCRUD>();
        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null);
        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            null, null, null, null);
        _environmentMock = new Mock<IWebHostEnvironment>();
        _userLevelMock = new Mock<ICRUDService<UserLevel>>();
        _userLessonServiceMock = new Mock<ICRUDService<UserLessons>>();
        _tariffsMock = new Mock<ICRUDService<Tariff>>();
        _twoFactorService = new TwoFactorService();
        _userExperienceMock = new Mock<ICRUDService<UserExperience>>();

        _controller = new AccountController(
            _userServiceMock.Object,
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _environmentMock.Object,
            _twoFactorService,
            _userLevelMock.Object,
            _userLessonServiceMock.Object,
            _tariffsMock.Object,
            _userExperienceMock.Object);
    }

    [Fact]
    public async Task Profile_UserExists_ReturnsViewWithModel()
    {
        // Arrange
        var user = new User { Id = 1, UserName = "TestUser" };
        _userManagerMock.Setup(um => um.GetUserId(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(user.Id.ToString());
        _userServiceMock.Setup(us => us.GetById(It.IsAny<int>())).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.IsInRoleAsync(user, "admin")).ReturnsAsync(true);

        // Act
        var result = await _controller.Profile(null);

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<UserProfileViewModel>(viewResult.Model);
        Assert.Equal(user.Id, model.User.Id);
    }

    [Fact]
    public async Task Profile_UserDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _userServiceMock.Setup(us => us.GetById(It.IsAny<int>()))
            .ReturnsAsync((User)null);
        // Act
        var result = await _controller.Profile(0);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Get_ReturnsViewWithModel()
    {
        // Arrange
        var user = new User { UserName = "TestUser", BirthDate = DateTime.Now };
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);

        // Act
        var result = await _controller.Edit();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<EditViewModel>(viewResult.Model);
        model.FullName = "TestUser";
        Assert.Equal(user.UserName, model.FullName);
    }

    [Fact]
    public async Task Register_Post_ValidModel_ReturnsJsonSuccess()
    {
        // Arrange
        var model = new RegisterViewModel
        {
            Email = "test@gmail.com",
            PhoneNumber = "0 777 88 89 99",
            District = "Ik",
            BirthDate = DateTime.Now.AddYears(-20),
            FirstName = "qw",
            LastName = "asd",
            UserName = "TestUser",
            Password = "Test@1234"
        };

        _userManagerMock.Setup(um => um.CreateAsync(It.IsAny<User>(), model.Password))
            .ReturnsAsync(IdentityResult.Success);
        _tariffsMock.Setup(t => t.GetAll()).Returns(new List<Tariff> { new Tariff { Name = "Start", Id = 1 } }.AsQueryable());

        // Act
        var result = await _controller.Register(model, null);

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var jsonString = JsonConvert.SerializeObject(jsonResult.Value);
        var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);
    
        Assert.NotNull(deserializedData); 
        Assert.True((bool)deserializedData["success"]); 
        Assert.Equal(model.Email, deserializedData["email"].ToString()); 
    }

    [Fact]
    public async Task Login_Post_ValidCredentials_ReturnsRedirectToProfile()
    {
        // Arrange
        var model = new LoginViewModel { LoginValue = "TestUser", Password = "Test@1234" };
        var user = new User { UserName = "TestUser" };

        _userManagerMock.Setup(um => um.FindByNameAsync(model.LoginValue)).ReturnsAsync(user);
        _signInManagerMock.Setup(sm => sm.PasswordSignInAsync(user, model.Password, false, false))
                          .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        // Act
        var result = await _controller.Login(model);

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Profile", redirectResult.ActionName);
    }

    [Fact]
    public async Task LogOut_Post_RedirectsToLogin()
    {
        // Act
        var result = await _controller.LogOut();

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", redirectResult.ActionName);
    }

    [Fact]
    public async Task ConfirmCode_ValidCode_ReturnsJsonSuccess()
    {
        // Arrange
        var model = new ConfirmCodeRequest { Email = "test@gmail.com", Code = "123456" };
        var user = new User { Email = model.Email, Id = 1 };

        _userManagerMock.Setup(um => um.FindByEmailAsync(model.Email)).ReturnsAsync(user);

        // Используем реальный сервис TwoFactorService
        var twoFactorService = new TwoFactorService();

        // Генерируем код для проверки
        var generatedCode = twoFactorService.GenerateCode(user.Id);

        // Создаем новый экземпляр AccountController с реальным TwoFactorService и всеми остальными параметрами
        var controller = new AccountController(
            _userServiceMock.Object,
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _environmentMock.Object,
            twoFactorService,
            _userLevelMock.Object,
            _userLessonServiceMock.Object,
            _tariffsMock.Object,
            _userExperienceMock.Object);

        var result = await controller.ConfirmCode(new ConfirmCodeRequest
        {
            Email = model.Email,
            Code = generatedCode // Передаем сгенерированный код
        });

        var jsonResult = Assert.IsType<JsonResult>(result);
        var jsonString = JsonConvert.SerializeObject(jsonResult.Value);
        Console.WriteLine($"JSON Result: {jsonString}");
        var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

        Assert.NotNull(deserializedData);
        Assert.True(deserializedData.ContainsKey("success"), "JSON does not contain 'success' key");
        Assert.True((bool)deserializedData["success"], "Expected success to be true but it was false");
    }
    
    [Fact]
    public async Task Delete_UserExists_DeletesUserAndRedirectsToRegister()
    {
        // Arrange
        var user = new User { UserName = "TestUser", AvatarUrl = "/userImages/test.png" };
        _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).ReturnsAsync(user);
        _userManagerMock.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);
        _environmentMock.Setup(e => e.WebRootPath).Returns("wwwroot");

        // Act
        var result = await _controller.Delete();

        // Assert
        var redirectResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Register", redirectResult.ActionName);
    }
}
