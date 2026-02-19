using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Interfaces;
using System.Reflection.Metadata.Ecma335;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ShoppingCartDbContext>();

builder.Services.AddControllersWithViews();

//Scoped Service => a different instance is created for every request
//Singleton Service => one instance to be shared by everyone

//builder.Services.AddScoped(typeof(ProductsRepository));


string productsFilePath = builder.Configuration["productJsonPath"].ToString();
 
string implementationChoice = builder.Configuration["dataSource"].ToString();

var host = builder.Environment;
string absoluteProductsFilePath =
    Path.Combine(host.ContentRootPath, productsFilePath);

builder.Services.AddScoped<CategoriesRepository>();
 
switch (implementationChoice)
{
    case "db":
        builder.Services.AddScoped<IProductsRepository, ProductsDbRepository>();
        break;

    case "json":
        builder.Services.AddScoped<IProductsRepository,
            ProductsFileRepository>(options => { return new ProductsFileRepository(absoluteProductsFilePath); });
        break;

    case "cache":
        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<IProductsRepository, ProductsCacheRepository>();
        break;
}
builder.Services.AddScoped<OrdersRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // https://localhost:xxxx/Products/Index
app.MapRazorPages();

app.Run();
