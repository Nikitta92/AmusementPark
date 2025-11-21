using Microsoft.EntityFrameworkCore;
using Model;
using Model.Data;
using Service.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddModel();



builder.Services.AddDbContextFactory<DatabaseContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("AmusementParkDB")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapVisitorEndpoints();

app.MapGet("test/db", () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        var a = dbContext.Database.ExecuteSql($"SELECT 1");
        return a;
    }
});

app.UseHttpsRedirection();

app.Run();