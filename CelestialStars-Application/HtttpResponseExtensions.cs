using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace CelestialStars_Application;

public static class HtttpResponseExtensions
{
    public static async Task WriteJsonAsync<T>(this HttpResponse response, T data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        response.ContentType = "application/json";
        response.StatusCode = statusCode.GetHashCode();

        var json = JsonConvert.SerializeObject(data,
                                               new JsonSerializerSettings
                                               {
                                                   Formatting = Formatting.Indented,
                                                   ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                                               });

        await response.WriteAsync(json);
    }
}