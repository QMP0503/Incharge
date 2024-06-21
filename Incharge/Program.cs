using BookAHotel.Logs;
using Incharge.Data;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Incharge.Models;
using Microsoft.AspNetCore.Identity;
using Incharge.Repository.IRepository;
using Incharge.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(Program));

var conectionString = builder.Configuration.GetConnectionString("InchargeDatabase");

builder.Services.AddDbContext<InchargeContext>(options =>
{
    options.UseMySql(conectionString, ServerVersion.AutoDetect(conectionString));
});

builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 4;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireLowercase = false;
    }
    ).AddEntityFrameworkStores<InchargeContext>().AddDefaultTokenProviders().AddRoles<IdentityRole>();


builder.Services.AddLog4net();


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IFindRepository<Client>, ClientRepository>();
builder.Services.AddScoped<IFindRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped<IFindRepository<Gymclass>, GymClassRepository>();
builder.Services.AddScoped<IFindRepository<Equipment>, EquipmentRepository>();
builder.Services.AddScoped<IFindRepository<Location>, LocationRepository>();

//add memory caching for client list and/or gym class list


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); //this don't exist anymore so change it
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
