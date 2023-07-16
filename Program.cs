using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentNDrive.Data;
using RentNDrive.ImplementationLogics.BusinessLogic;
using RentNDrive.ImplementationLogics.IBusinessLogic;
using RentNDrive.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
//builder.Services.AddIdentity<UserInfo, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                //.AddDefaultTokenProviders()
                //.AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<UserInfo, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUser, UserRep>();
builder.Services.AddScoped<IVehType, AdminRep>();
builder.Services.AddScoped<IVehManufacturer, AdminRep>();
builder.Services.AddScoped<IVehicle, VehicleRep>();
//builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Authentication/LogIn");
builder.Services.AddRazorPages();
//builder.Services.Configure<IdentityOptions>(options =>
//{
//    // Default Lockout settings.   
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//    options.Lockout.MaxFailedAccessAttempts = 5;
//    options.Lockout.AllowedForNewUsers = true;
//});
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=DefaultAdminandRoles}/{id?}");
app.MapRazorPages();

app.Run();
