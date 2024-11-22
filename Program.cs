using LibraryApp.Data;
using LibraryApp.Repositories;
using LibraryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IBookRepository>(provider => new BookRepository(Path.Combine(builder.Environment.ContentRootPath, "Data", "books.json")));
builder.Services.AddSingleton<IBorrowTransactionRepository>(provider => new BorrowTransactionRepository(Path.Combine(builder.Environment.ContentRootPath, "Data", "transactions.json")));

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(
    builder.Configuration.GetConnectionString("localDb")));

builder.Services.AddControllers();
builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromSeconds(30);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();
