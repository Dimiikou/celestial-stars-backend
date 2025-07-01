using System.Text.Json;

namespace CelestialStars_Api.services;

public static class JsonHelper
{
    public static async Task<string> GetJsonPropertyValueAsync(HttpContext httpContext, string propertyName)
    {
        using var reader = new StreamReader(httpContext.Request.Body);
        var requestBody = await reader.ReadToEndAsync();

        var jsonDocument = JsonDocument.Parse(requestBody);
        if (jsonDocument.RootElement.TryGetProperty(propertyName, out var propertyValue))
        {
            return propertyValue.ToString();
        }

        throw new Exception("Property nicht gefunden");
    }
}