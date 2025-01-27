namespace CelestialStars_Api.accounting;

public static class AccountApi
{
    public static void MapAccountApi(this IEndpointRouteBuilder routeBuilder)
    {
        var accountGroup = routeBuilder.MapGroup("/account");

        accountGroup.MapGet("/", GetLogin);
    }

    private static Task GetLogin(HttpContext context)
    {
        throw new NotImplementedException();
    }
}