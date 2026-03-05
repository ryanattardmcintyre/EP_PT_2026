using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Common.Interfaces;
using System.Reflection.Metadata.Ecma335;
using DataAccess.Utilities;
using DataAccess.Factory;
using Common.Models;
using Presentation.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ShoppingCartDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
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

// DI with Interfaces
// Choosing Implementation A OR B OR C
builder.Services.AddScoped<CategoriesRepository>();
builder.Services.AddScoped<NotificationFactory>();

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



//DI with Interfaces using KEYED SCOPED
//Inject Implementation A AND B
//Ex1
builder.Services.AddKeyedScoped<IOrdersRepository, OrdersRepository>("db");
builder.Services.AddKeyedScoped<IOrdersRepository, OrdersCacheRepository>("cache");
//Ex2
builder.Services.AddKeyedScoped<IPriceCalculation, VatCalculation>("vat");
builder.Services.AddKeyedScoped<IPriceCalculation, BlackFridayCalculation>("blackFriday");

//DI - scoped service

builder.Services.AddScoped<ProductCreateValidationFilter>();

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
