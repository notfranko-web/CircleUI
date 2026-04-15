using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using CircleUI.Data;
using CircleUI.Data.Models;
using CircleUI.Data.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

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
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    const string adminEmail = "admin@circleui.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new User { UserName = adminEmail, Email = adminEmail, DisplayName = "Admin" };
        await userManager.CreateAsync(adminUser, "Admin1234!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    await BootstrapComponentSeeder.SeedAsync(dbContext);
}

app.Run();