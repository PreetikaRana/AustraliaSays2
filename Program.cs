using AustraliaSays2_DataAccess.Data;
using AustraliaSays2_DataAccess.Repository;
using AustraliaSays2_DataAccess.Repository.IRepository;

using AustraliaSays2_Models.Models;
using AustraliaSays2_Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

string cs = builder.Configuration.GetConnectionString("constr");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cs));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<ISiteTypeRepository, SiteTypeRepository>();
builder.Services.AddScoped<IArticleTypeRepository, ArticleTypeRepository>();
builder.Services.AddScoped<IUserRegisterRepository, UserRegisterRepository>();
builder.Services.AddScoped<ILoginUserRepository, LoginUserRepository>();
builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<HomeRepository>();
builder.Services.AddScoped(typeof(PaginatedList<>));
builder.Services.AddSingleton<GenerateTokenAsync>();

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));

var app = builder.Build();
using (var scope =app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await Seed.Initialize(services);

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured while seeding data");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");
});
app.Run();
