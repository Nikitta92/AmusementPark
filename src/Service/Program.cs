using Model;
using Service;
using Service.Controllers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddModel();


builder.Services.AddEf(builder.Configuration);
//builder.Services.AddDapper(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapEndpoints();

app.UseHttpsRedirection();

app.Run();