using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using LibrarySystem.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using React.AspNet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IBookRepository, EFBookRepository>();
builder.Services.AddTransient<IUserRepository, EFUserRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMvc();
builder.Services.AddReact(); // Подключение React
builder.Services.AddJsEngineSwitcher(op => // Подключение движка  JS - ChakraCore
    op.DefaultEngineName = ChakraCoreJsEngine.EngineName).AddChakraCore();

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LibraryDbContext>(s => s.UseSqlServer(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = new PathString("/Account/Login"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseStatusCodePages();
app.UseReact(config => { });
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: null,
    pattern: "",
    defaults: new { controller = "Home", action = "Index" });

app.MapControllerRoute(name: default, pattern: "{controller}/{action}/{id?}");

app.Run();
