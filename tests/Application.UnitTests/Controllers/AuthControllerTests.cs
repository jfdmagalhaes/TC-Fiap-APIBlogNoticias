using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using WebApi.Controllers;

namespace Application.UnitTests.Controllers;
public class AuthControllerTests
{
    private readonly AuthController _authController;
    private readonly Mock<UserManager<IdentityUser>> _userManager = new();
    private readonly Mock<SignInManager<IdentityUser>> _signInManager = new();
    private readonly Mock<IUserAuthenticationRepository> _repository = new();

    public AuthControllerTests()
    {
        _userManager = new Mock<UserManager<IdentityUser>>();
        _signInManager = new Mock<SignInManager<IdentityUser>>();

        _userManager = new Mock<UserManager<IdentityUser>>(
            new Mock<UserManager<IdentityUser>>().Object,
            null, null, null, null, null, null, null, null
        );

        _signInManager = new Mock<SignInManager<IdentityUser>>();
          new Mock<SignInManager<IdentityUser>>(
                    _userManager.Object, null, null, null, null, null, null
            );


        _authController = new AuthController(
            _userManager.Object,
            _signInManager.Object,
            _repository.Object);
    }

    //[Test]
    //public async Task Login_ShouldExecute_Successfully()
    //{
    //    //arr
    //    var createLoginModel = new LoginViewModel { Password = "password", UserName = "userName" };

    //    _userManager.Setup(x => x.FindByNameAsync(createLoginModel.UserName))
    //        .ReturnsAsync(It.IsAny<IdentityUser>);

    //    _signInManager.Setup(x => x.PasswordSignInAsync(createLoginModel.UserName, createLoginModel.Password, false, false))
    //        .ReturnsAsync(It.IsAny<SignInResult>);

    //    //act
    //    var login = await _authController.Login(createLoginModel);

    //    //ass
    //    login.Should().NotBeNull();
    //}

    [Test]
    public async Task Login_ValidCredentials_ReturnsOk()
    {
        // Arrange
        var userName = "usuario_teste";
        var password = "senha_teste";

        var userManagerMock = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object,
            null, null, null, null, null, null, null, null
        );

        var signInManagerMock = new Mock<SignInManager<IdentityUser>>(
            userManagerMock.Object, null, null, null, null, null, null
        );

        //var repositoryMock = new Mock<SuaClasseDeRepositorio>(); // Substitua pelo seu repositório real
        //repositoryMock.Setup(repo => repo.CreateTokenAsync(It.IsAny<IdentityUser>()))
        //    .ReturnsAsync("seu_token_de_teste");

        //var controller = new SuaController(userManagerMock.Object, signInManagerMock.Object, repositoryMock.Object);

        //var model = new LoginViewModel
        //{
        //    UserName = userName,
        //    Password = password
        //};

        //signInManagerMock.Setup(signInManager => signInManager.PasswordSignInAsync(userName, password, false, false))
        //    .ReturnsAsync(SignInResult.Success);

        //// Act
        //var result = await controller.Login(model) as ObjectResult;

        //// Assert
        //Assert.IsNotNull(result);
        //Assert.AreEqual(200, result.StatusCode);
        //Assert.AreEqual("seu_token_de_teste", (result.Value as dynamic).Token);
    }

}