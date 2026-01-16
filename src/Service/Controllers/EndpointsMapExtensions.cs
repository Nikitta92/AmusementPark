namespace Service.Controllers;

public static class EndpointsMapExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapVisitorEndpoints();
    }
}