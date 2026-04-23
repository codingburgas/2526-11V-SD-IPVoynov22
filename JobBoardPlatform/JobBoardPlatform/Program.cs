using Microsoft.EntityFrameworkCore;
using JobBoardPlatform.Data;
using JobBoardPlatform.Services;
using JobBoardPlatform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Service Registration
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); 

// Register Database Context for SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Configuration (Authentication & Authorization)
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6; 
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>() // Required for Role-based access (Admin/Employer)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Dependency Injection for Custom Services
builder.Services.AddScoped<IJobService, JobService>();

var app = builder.Build();

// Database & Identity Initialization (Seeding)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        // Automatically create database and tables if they don't exist
        context.Database.EnsureCreated(); 

        // Seed Roles
        string[] roleNames = { "Admin", "Employer", "Candidate" };
        foreach (var roleName in roleNames)
        {
            if (!roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }
        }

        // Seed Default Admin Account 
        string adminEmail = "ivko@abv.bg";
        string adminPassword = "123456"; 

        var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
        
        if (adminUser == null)
        {
            adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var createResult = userManager.CreateAsync(adminUser, adminPassword).GetAwaiter().GetResult();
            
            if (createResult.Succeeded)
            {
                userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
                Console.WriteLine("--- SUCCESS: Admin profile created (ivko@abv.bg / 123456) ---");
            }
        }
        else if (!userManager.IsInRoleAsync(adminUser, "Admin").GetAwaiter().GetResult())
        {
            userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
        }

        Console.WriteLine("--- Database and Roles initialized successfully! ---");
    }
    catch (Exception ex)
    {
        Console.WriteLine("--- DATABASE ERROR: " + ex.Message);
    }
}

// Middleware Pipeline (HTTP Request Handling)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Authentication must always be called BEFORE Authorization
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Enables Identity-related routes (Login/Register)

app.Run();