using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;

namespace Application.UnitTests.Controllers;
public class AuthControllerTests
{
    private readonly AuthController _authController;
    private readonly Mock<UserManager<IdentityUser>> _mockUserManager = new();
    private readonly Mock<SignInManager<IdentityUser>> _mockSignInManager = new();
    private readonly Mock<IUserAuthenticationRepository> _mockRepository = new();

    public AuthControllerTests()
    {
        _mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
        _mockSignInManager = new Mock<SignInManager<IdentityUser>>(_mockUserManager.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(), null, null, null, null);
        _mockRepository = new Mock<IUserAuthenticationRepository>();

        _authController = new AuthController(_mockUserManager.Object, _mockSignInManager.Object, _mockRepository.Object);
    }

    [Test]
    public async Task Login_ShouldExecute_Successfully()
    {
        //arr
        var createLoginModel = new LoginViewModel { Password = "password", UserName = "userName" };
        var identityUser = new IdentityUser { UserName = createLoginModel.UserName };
        

        _mockUserManager.Setup(x => x.FindByNameAsync(createLoginModel.UserName))
            .ReturnsAsync(identityUser);

        _mockSignInManager.Setup(x => x.PasswordSignInAsync(createLoginModel.UserName, createLoginModel.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        //act
        var login = await _authController.Login(createLoginModel);

        //ass
        login.Should().NotBeNull();
    }

    [Test]
    public async Task CreateUser_ShouldExecute_Successfully()
    {
        //Arrange
        var model = new CreateUserViewModel
        {
            UserName = "userName",
            Password = "password"
        };

        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<IdentityUser>(), model.Password))
            .ReturnsAsync(IdentityResult.Success);

        //Act
        var result = await _authController.CreateUser(model) as OkObjectResult;

        //Assert
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(200));
    }

    [Test]
    public async Task Login_InvalidLogin_ReturnsUnauthorizedResult()
    {
        //arr
        var model = new LoginViewModel {UserName = "userName", Password = "invalidpassword" };

        _mockUserManager.Setup(x => x.FindByNameAsync(model.UserName))
            .ReturnsAsync(new IdentityUser());

        _mockSignInManager.Setup(x => x.PasswordSignInAsync(model.UserName, model.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        //act
        var result = await _authController.Login(model) as UnauthorizedObjectResult;

        //ass
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(401));
    }

    [Test]
    public async Task CreateUser_InvalidModel_ReturnsBadRequestResult()
    {
        //arr
        var model = new LoginViewModel { UserName = "userName", Password = "invalidpassword" };

        _mockUserManager.Setup(x => x.FindByNameAsync(model.UserName))
            .ReturnsAsync(new IdentityUser());

        _mockSignInManager.Setup(x => x.PasswordSignInAsync(model.UserName, model.Password, false, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

        //act
        var result = await _authController.Login(model) as UnauthorizedObjectResult;

        //ass
        Assert.IsNotNull(result);
        Assert.That(result.StatusCode, Is.EqualTo(401));
    }
}