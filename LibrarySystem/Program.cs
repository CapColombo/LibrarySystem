using LibrarySystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IBookRepository, EFBookRepository>();
builder.Services.AddTransient<IUserRepository, EFUserRepository>();
builder.Services.AddMvc();
builder.Services.AddMemoryCache();
builder.Services.AddSession();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LibraryDbContext>(s => s.UseSqlServer(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = new PathString("/Account/Login"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseStatusCodePages();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: null,
    pattern: "",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(name: default, pattern: "{controller}/{action}/{id?}");

app.Run();
