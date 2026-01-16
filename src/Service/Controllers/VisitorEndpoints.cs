using Model.Services;
using Service.Contracts.Requests;
using Service.Mapping;

namespace Service.Controllers;

public static class VisitorEndpoints
{
    public static void MapVisitorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("visitor");
        
        group.MapPost("/", async (IVisitorService visitorService, VisitorCreateRequest visitorCreateRequest) =>
        {
            var result = await visitorService.CreateAsync(visitorCreateRequest.ToModel());
            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.ValidationProblem(result.Errors!);
        }).WithName("CreateVisitor");
        
        group.MapGet("/{id:int}", async (IVisitorService visitorService, int id) =>
        {
            var result = await visitorService.TryGetAsync(id);
            
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).WithName("ReadVisitor");

        group.MapPut("/", async (IVisitorService visitorService, VisitorUpdateRequest visitorUpdateRequest) =>
        {
            var existedVisitor = await visitorService.TryGetAsync(visitorUpdateRequest.Id);

            if (existedVisitor is null)
                return Results.NotFound();
            
            var result = await visitorService.UpdateAsync(visitorUpdateRequest.ToModel());

            return result.IsSuccess
                ? Results.Ok(result.Value!.ToResponse())
                : Results.ValidationProblem(result.Errors!);
        }).WithName("UpdateVisitor");
        
        group.MapDelete("/{id:int}", async (IVisitorService visitorService, int id) =>
        {
            var result = await visitorService.DeleteAsync(id);

            return result
                ? Results.NoContent()
                : Results.NotFound();
        }).WithName("DeleteVisitor");
        
        group.MapGet("/", async (IVisitorService visitorService) => // TODO: Add paging
        {
            var result = await visitorService.GetAll();

            return result;
        }).WithName("ReadAllVisitors");
    }
}