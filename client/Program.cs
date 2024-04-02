var builder = WebApplication.CreateBuilder(args);

// Добавление служб
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Конфигурация HTTP пайплайна
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Создание маршрута по умолчанию для контроллера Home
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
