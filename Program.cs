using Inventory_Manager.Data;
using Inventory_Manager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.AddControllersWithViews();

var app = builder.Build();

//Initialize app secrets
var configuration = app.Services.GetService<IConfiguration>();
var hosting = app.Services.GetService<IWebHostEnvironment>();

//if (hosting.IsDevelopment())
//{
//    var secrets = configuration.GetSection("Secrets").Get<AppSecrets>();
//    DbInitializer.appSecrets = secrets;
//}

// see user data
using (var scope = app.Services.CreateScope())
{
    DbInitializer.SeedUsersAndRoles(scope.ServiceProvider).Wait();
}
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

// to prevent -X-Content-Type-Options Missing
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx => ctx.Context.Response.Headers.Add("X-Content-Type-Options", "nosniff")
});

app.Use(async (context, next) =>
{
    // to prevent Anti-clickjacking Header (aka.X-Frame-Options Header Not Set)
    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
    // to prevent -XSS Protection
    context.Response.Headers.Add("X-Xss-Protection", "1");
    // to prevent X-Content-Type-Options MissingX
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Cache-Control", "no-cache,no-store,must-revalidate");
    await next();
});

// to prevent cookie http only 
app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
