
using srv1.ServerData;
using System.Drawing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/display", () =>
{ 
    var bounds = Screen.GetSize();
    return new DisplayInfo() { Width = bounds.Width, Height = bounds.Height };
})
.WithName("GetDisplayWidth")
.WithOpenApi();

app.MapPost("/pixel", (Point point) =>
{
    return Screen.GetColorAt(point);
})
.WithName("GetPixelColor")
.WithOpenApi();

app.Run();
