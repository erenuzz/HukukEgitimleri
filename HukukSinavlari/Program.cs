using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.EntityFramework;
using Entity.Entities;
using HukukSinavlari.Helper;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DbHukukEgitim>();


builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<DbHukukEgitim>();


builder.Services.AddTransient<IEducationDal, EfEducationDal>();
builder.Services.AddTransient<EducationManager>();


builder.Services.AddTransient<IPurchasedTrainingsDal, EfPurchasedTrainingsDal>();
builder.Services.AddTransient<PurchasedTrainingsManager>();

builder.Services.AddTransient<ITrainerLessonsDal, EfTrainerLessonsDal>();
builder.Services.AddTransient<TrainerLessonsManager>();

builder.Services.AddTransient<ITrainingHoursDal, EfTrainingHoursDal>();
builder.Services.AddTransient<TrainingHoursManager>();

builder.Services.AddScoped<IpStackLocationService>();

builder.Services.AddSession();
builder.Services.AddRazorPages();



builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 1;    
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
	 name: "Areas",
	 areaName: "Control",
	  pattern: "Control/{controller=Panel}/{action=Index}/{id?}"
	);

app.Run();
