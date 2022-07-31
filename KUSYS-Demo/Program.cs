using KUSYS_Demo.Models;
using KUSYS_Demo.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("KUSYSAppDB");
builder.Services.AddDbContext<KUSYSDBContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<KUSYSDBContext>();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

/// <summary>
/// Created to get IServiceScope for DB contex operations.
/// </summary>
using (var scope = app.Services.CreateScope())
{
    SeedData.SeedDatabase(scope.ServiceProvider);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();



