using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CelestialStars_Application.users.register;
using CelestialStars_Domain.dataTransferObjects;

namespace CelestialStars_UnitTests;

public class AccountApiTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public AccountApiTests(CustomWebApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task RegisterUser_ShouldReturnCreatedUserAndSetCookie()
    {
        // Arrange
        var registerDto = new RegisterUserRequest
        {
            Username = "Dimiikou",
            Email = "leon@aissa.dev",
            Password = "1234!g"
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/account/register", registerDto);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Validate Response Data
        var responseContent = await response.Content.ReadAsStringAsync();
        var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
        var user = jsonResponse.GetProperty("user");
        var message = jsonResponse.GetProperty("message").GetString();

        Assert.Equal("Dimiikou", user.GetProperty("username").ToString());
        Assert.Equal("leon@aissa.dev", user.GetProperty("email").ToString());
        Assert.NotNull(user.GetProperty("id").ToString());
        Assert.Equal("Registration successful", message);

        Assert.True(response.Headers.Contains("Set-Cookie"));
    }
}