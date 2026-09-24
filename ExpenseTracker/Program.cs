using ExpenseTracker.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Creates expenses.db automatically on first run (no migrations needed)
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Expenses/Index");
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Expenses}/{action=Index}/{id?}");

app.Run();
