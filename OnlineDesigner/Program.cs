using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineDesigner.Controllers;
using OnlineDesigner.Data;
using OnlineDesigner.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<OnlineDesignerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OnlineDesignerContext") ?? throw new InvalidOperationException("Connection string 'OnlineDesignerContext' not found.")));

// Add services to the container.
builder.Services.AddTransient<TypesController, TypesController>();
builder.Services.AddTransient<ItemsController, ItemsController>();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddScoped<IdentityContext>();
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<OnlineDesignerContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 0;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
