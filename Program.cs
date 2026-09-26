using Microsoft.EntityFrameworkCore;
using ServiceRepoDemo.Data;
using ServiceRepoDemo.Repositories;
using ServiceRepoDemo.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository layer (data access)
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Service layer (business logic)
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Products/Index");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
