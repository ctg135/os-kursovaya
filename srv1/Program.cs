
using srv1.ServerData;
using System.Drawing;

// Проверка на создание второго экземпляра
bool cn;
Mutex m = new Mutex(true, "srv1", out cn);
if(!m.WaitOne(0, false))
    return;

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

// Маршрут для получения разршения основного монитора
app.MapGet("/display", () =>
{ 
    var bounds = Screen.GetSize();
    return new DisplayInfo() { Width = bounds.Width, Height = bounds.Height };
})
.WithName("GetDisplayWidth")
.WithOpenApi();

// Маршрут для получения цвета пикселя по координате
app.MapPost("/pixel", (Point point) =>
{
    return Screen.GetColorAt(point);
})
.WithName("GetPixelColor")
.WithOpenApi();

// Запуск приложения
app.Run();
