using Model.Domain;
using Model.Services;

namespace Service.Controllers;

public static class VisitorEndpoints
{
    public static void MapVisitorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("visitor");
        
        group.MapGet("/{id:int}", async (IVisitorService visitorService, int id) =>
        {
            var result = await visitorService.TryGetAsync(id);
            
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).WithName("GetVisitor");

        group.MapPost("/", async (IVisitorService visitorService, Visitor visitor) =>
        {
            var result = await visitorService.CreateAsync(visitor);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.ValidationProblem(result.Errors!);
        }).WithName("CreateVisitor");
    }
}