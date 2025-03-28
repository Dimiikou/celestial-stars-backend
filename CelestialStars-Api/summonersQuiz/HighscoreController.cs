using CelestialStars_Domain;
using CelestialStars_Domain.dataTransferObjects;
using CelestialStars_Sql;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CelestialStars_Api.summonersQuiz;

public static class HighscoreController
{
    public static void MapHighscoreApi(this IEndpointRouteBuilder routeBuilder)
    {
        var accountGroup = routeBuilder.MapGroup("/account")
            .RequireAuthorization();

        accountGroup.MapPost("/", InsertHighscore)
            .AllowAnonymous();
        accountGroup.MapGet("/", GetHighScores)
            .AllowAnonymous();
    }

    private static async Task<IResult> InsertHighscore(HttpContext context, HighscoreDto highScoreDto, CelestialStarsDbContext db)
    {
        var highScore = new Highscore()
        {
            Username = highScoreDto.Username,
            Score = highScoreDto.Score
        };

        db.Highscores.Add(highScore);
        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            user = new { highScore.Id, highScore.Username, highScore.Score },
            message = "Registration successful"
        });
    }

    private static async Task GetHighScores(HttpContext httpContext, CelestialStarsDbContext dbContext)
    {
        var highScores = await dbContext.Highscores.ToListAsync();

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = 200;

        var json = JsonConvert.SerializeObject(highScores,
                                               new JsonSerializerSettings
                                               {
                                                   Formatting = Formatting.Indented,
                                                   ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                                               });

        await httpContext.Response.WriteAsync(json);
    }
}