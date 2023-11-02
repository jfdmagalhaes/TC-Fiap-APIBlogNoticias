using BlogNoticias.IntegrationTests.Factory;
using Infrastructure.EntityFramework.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Json;
using WebApi;
using WebApi.Controllers;

namespace BlogNoticias.IntegrationTests.Utils;
public class IntegrationTestTools
{
    private readonly CustomWebApplicationFactory<Startup> _application;
    private readonly HttpClient _httpClient;

    public IntegrationTestTools(CustomWebApplicationFactory<Startup> application)
    {
        _application = application;
        _httpClient = application.CreateClient();
    }

    public async Task<HttpClient> LoginUser()
    {
        var user = new IdentityUser { UserName = "teste", Email = "email" };
        var password = "123Abc@";

        await Createuser(_application, user, password);

        var userModel = new CreateUserViewModel { UserName = user.UserName, Password = password };
        var httpResponse = await _httpClient.PostAsJsonAsync($"/api/Auth/login", userModel);

        var content = await httpResponse.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<TokenResponse>(content);

        _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + result!.Token);

        return _httpClient;
    }

    public async Task Createuser(CustomWebApplicationFactory<Startup> _application, IdentityUser user, string password)
    {

        using (var scope = _application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            var userManager = provider.GetRequiredService<UserManager<IdentityUser>>();
            await userManager.CreateAsync(user, password);

            using (var applicationDbContext = provider.GetRequiredService<ApplicationDbContext>())
            {
                await applicationDbContext.Database.EnsureCreatedAsync();
                await applicationDbContext.SaveChangesAsync();
            }
        }
    }
}

public class TokenResponse
{
    public string Token { get; set; }
}