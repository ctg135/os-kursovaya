using System.Diagnostics;
using srv2.ServerData;

// Проверка на создание второго экземпляра программы
bool cn;
Mutex m = new Mutex(true, "srv2", out cn);
if(!m.WaitOne(0, false))
    return;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Конфигурация HTTP пайплайна
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Маршрут для получения информации с сервера
app.MapGet("/handle", () =>
{
    // Получение и передача системной информации
    var process = Process.GetCurrentProcess();
    return new Handles() {
        MainHandle = process.MainWindowHandle,
        ProcessHandle = process.Handle
    };
})
.WithName("GetHandles")
.WithOpenApi();

// Запуск приложения
app.Run();

namespace srv2.ServerData
{
    public record Handles
    {
        public long MainHandle { get; init; }
        public long ProcessHandle { get; init; }
    }
}
