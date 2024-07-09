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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.Build.Framework;

// Set your Cloudinary credentials
//=================================



var builder = WebApplication.CreateBuilder(args);


DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] {"C:\\Users\\intern.pmquang1\\C#\\Incharge\\Incharge\\Incharge.env"}));
Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
cloudinary.Api.Secure = true;

builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(Program));



var conectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

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
builder.Services.AddScoped<IFindRepository<BusinessReport>, BusinessReportRepository>();
builder.Services.AddScoped<IFindRepository<Expense>, ExpensesRepository>();

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

builder.Services.AddScoped<IService<SaleVM, Sale>, SalesService>(); 
builder.Services.AddScoped<IPagingService<PaginatedList<Sale>>, SalesPagingService>(); 

builder.Services.AddScoped<IBusinessReportService, BusinessReportService>(); //no paging yet

builder.Services.AddScoped<IService<ExpenseVM, Expense>, ExpenseService>(); 
builder.Services.AddScoped<IPagingService<PaginatedList<Expense>>, ExpensesPagingService>(); 

//dropdown menu view options
builder.Services.AddScoped<IDropDownOptions<ProductVM>, ProductService>();
builder.Services.AddScoped<IDropDownOptions<SaleVM>, SalesService>();
builder.Services.AddScoped<IDropDownOptions<EmployeeVM>, EmployeeService>();

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

app.MapControllerRoute( //edit to make all pages require authentication
    name: "default",
    pattern: "{controller=Location}/{action=Index}/{id?}");


    //check if they put remember me will it work..
    //pattern: "{controller=Employee}/{action=Index}/{id?}"); //for testing purposes
using(var scope = app.Services.CreateScope())
{
    var initializationService = scope.ServiceProvider.GetRequiredService<IBusinessReportService>();
    initializationService.AddService(); //run to add new business report if new month begins
}

app.Run();
