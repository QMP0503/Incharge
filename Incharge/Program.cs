using BookAHotel.Logs;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Incharge.Repository.IRepository;
using Incharge.Repository;
using Incharge.Service.IService;
using Incharge.Service;
using Incharge.Service.PagingService;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
    }).AddEntityFrameworkStores<InchargeContext>().AddDefaultTokenProviders().AddRoles<IdentityRole>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddLog4net();

//Repository injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IFindRepository<Client>, ClientRepository>();
builder.Services.AddScoped<IFindRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped<IFindRepository<Gymclass>, GymClassRepository>();
builder.Services.AddScoped<IFindRepository<Equipment>, EquipmentRepository>();
builder.Services.AddScoped<IFindRepository<Location>, LocationRepository>();
builder.Services.AddScoped<IFindRepository<Product>, ProductRepository>();
builder.Services.AddScoped<IFindRepository<EmployeeType>, EmployeeTypeRepository>();
builder.Services.AddScoped<IFindRepository<Producttype>, ProductTypeRepository>();
builder.Services.AddScoped<IFindRepository<Sale>, SalesRepository>();
builder.Services.AddScoped<IFindRepository<Discount>, DiscountRepository>();

//service injection
builder.Services.AddScoped<IService<ClientVM, Client>, ClientService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Client>>, ClientPagingService>(); //figure out async when all is working

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Employee>>, EmployeePagingService>();

builder.Services.AddScoped<IService<EquipmentVM,Equipment>, EquipmentService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Equipment>>, EquipmentPagingService>();

builder.Services.AddScoped<IService<LocationVM,Location>, LocationService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Location>>, LocationPagingService>();

builder.Services.AddScoped<IService<GymClassVM, Gymclass>, GymClassService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Gymclass>>, GymclassPagingService>();

builder.Services.AddScoped<IService<ProductVM, Product>, ProductService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Product>>, ProductPagingService>();

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

using (var scope = app.Services.CreateScope()) //add roles for manager/employee below.
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var role = "Admin"; //use array to add more roles

    //use foreach(roles in roles to check and add)
    if (!await roleManager.RoleExistsAsync(role)) await roleManager.CreateAsync(new IdentityRole(role));

    // SeedData.Initialize(services);
}
using (var scope = app.Services.CreateScope())
{
    string Email = "admin@admin.com";
    var password = "1234";


    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    if (await userManager.FindByEmailAsync(Email) == null)
    {
        var adminUser = new User();
        adminUser.FirstName = "Admin";
        adminUser.LastName = "Admin";
        adminUser.UserName = Email;
        adminUser.Email = Email;

        await userManager.CreateAsync(adminUser, password);
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

}


app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); //check if they put remember me will it work..
    //pattern: "{controller=Employee}/{action=Index}/{id?}"); //for testing purposes

app.Run();
