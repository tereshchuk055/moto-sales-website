using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using MotoShop.Data;
using MotoShop.Constants;
using MotoShop.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    }) ;

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RolePolicy", policy =>
    {
        policy.Requirements.Add(new RoleRequirement(UserRole.Admin.ToString(), UserRole.Moderator.ToString()));
    });
});

builder.Services.ConfigureDatabase();

builder.Services.AddScoped<IPhotoService, PhotoService>();
var conf = builder.Configuration.GetSection("CloudinarySettings");
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
