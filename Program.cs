using Microsoft.EntityFrameworkCore;
using PoliRiwi.Data;
using PoliRiwi.Services;
using PoliRiwi.Services.Interfaces;
using FluentValidation;
using PoliRiwi.Models;
using PoliRiwi.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// The builder to set parameters to connect to the database 
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<MySqlDbContext>(options => 
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Dependency Injection
builder.Services.AddScoped<IValidator<Users>, UserValidators>();
builder.Services.AddScoped<IValidator<Places>, PlaceValidators>();
builder.Services.AddScoped<IValidator<Reservations>, ReservationValidatiors>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPlaceServices, PlaceService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.UseStaticFiles(); 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Users}/{id?}")
    .WithStaticAssets();


app.Run();
