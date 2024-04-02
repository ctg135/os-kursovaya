using System.Diagnostics;
using srv2.ServerData;

bool cn;
Mutex m = new Mutex(true, "srv1", out cn);
if(!m.WaitOne(0, false))
    return;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/process", () =>
{
    return Process.GetCurrentProcess().Id;
})
.WithName("GetProcessId")
.WithOpenApi();

app.MapGet("/handle", () =>
{
    var process = Process.GetCurrentProcess();
    return new Handles() {
        MainHandle = process.MainWindowHandle,
        ProcessHandle = process.Handle
    };
})
.WithName("GetHandles")
.WithOpenApi();


app.Run();

namespace srv2.ServerData
{
    public record Handles
    {
        public long MainHandle { get; init; }
        public long ProcessHandle { get; init; }
    }
}
