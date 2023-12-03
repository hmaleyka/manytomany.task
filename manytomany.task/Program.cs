using manytomany.task.DAL;
using manytomany.task.Services;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


//builder.Services.AddSession(opt=>
//{
//    opt.IdleTimeout=TimeSpan.FromSeconds(5);
//});

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));


});

builder.Services.AddScoped<LayoutService>();

var app = builder.Build();

//app.UseSession();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}"

    ); 
app.UseStaticFiles();

app.Run();
