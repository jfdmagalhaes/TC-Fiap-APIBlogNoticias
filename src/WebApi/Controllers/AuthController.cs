using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserAuthenticationRepository _repository;

    public AuthController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IUserAuthenticationRepository repository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _repository = repository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Ok(new { Token = await _repository.CreateTokenAsync(user) });
        }
        else
        {
            return Unauthorized(new { message = "Nome de usuário ou senha incorretos." });
        }
    }

    [HttpPost("createUser")]
    public async Task<IActionResult> CreateUser(CreateUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.UserName, Email = model.UserName };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Usuário registrado com sucesso
                return Ok(new { message = "Usuário registrado com sucesso." });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        // Falha no registro
        return BadRequest(ModelState);
    }
}

public class LoginViewModel
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}

public class CreateUserViewModel : LoginViewModel
{ }