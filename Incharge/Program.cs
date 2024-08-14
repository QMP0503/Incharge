using BookAHotel.Logs;
using Incharge.Data;
using Microsoft.EntityFrameworkCore;
using Incharge.Models;
using Incharge.ViewModels;
using Microsoft.AspNetCore.Identity;
using Incharge.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Incharge.Repository;
using Incharge.Service.IService;
using Incharge.Service;
using Incharge.Service.PagingService;
using dotenv.net;
using Incharge.Helper;
using MyMovies.Models;


var builder = WebApplication.CreateBuilder(args);



DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] {"C:\\Users\\intern.pmquang1\\C#\\Incharge\\Incharge\\Incharge.env"}));


builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(Program));

//Cloudinary injection
builder.Services.AddScoped <IPhotoService, PhotoService>();

builder.Services.Configure<CloudinarySettings>( options =>
{
    options.CloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
    options.ApiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
    options.ApiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");
});

var conectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

builder.Services.AddDbContext<InchargeContext>(options =>
{
    options.UseMySql(conectionString, ServerVersion.AutoDetect(conectionString));
});

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Account/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
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

builder.Services.AddScoped<IService<EmployeeVM, Employee>, EmployeeService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Employee>>, EmployeePagingService>();

builder.Services.AddScoped<IService<EquipmentVM,Equipment>, EquipmentService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Equipment>>, EquipmentPagingService>();

builder.Services.AddScoped<IService<LocationVM,Location>, LocationService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Location>>, LocationPagingService>();

builder.Services.AddScoped<IService<GymClassVM, Gymclass>, GymClassService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Gymclass>>, GymclassPagingService>();

builder.Services.AddScoped<IService<ProductVM, Product>, ProductService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Product>>, ProductPagingService>();

builder.Services.AddScoped<IBusinessReportService, BusinessReportService>(); 
builder.Services.AddScoped<IPagingService<PaginatedList<BusinessReport>>, BusinessReportPagingService>();

builder.Services.AddScoped<IService<ExpenseVM, Expense>, ExpenseService>(); 
builder.Services.AddScoped<IPagingService<PaginatedList<Expense>>, ExpensesPagingService>(); 
builder.Services.AddScoped<IRecurringExpenseService, ExpenseService>();

builder.Services.AddScoped<IService<DiscountVM, Discount>, DiscountService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Discount>>, DiscountPagingService>();

//Sale Service with payment confirmation
builder.Services.AddScoped<IService<SaleVM, Sale>, SalesService>();
builder.Services.AddScoped<IPagingService<PaginatedList<Sale>>, SalesPagingService>();
builder.Services.AddScoped<IConfirmation<SaleVM>, SalesService>();

//Dropdown menu view options
builder.Services.AddScoped<IDropDownOptions<ProductVM>, ProductService>();
builder.Services.AddScoped<IDropDownOptions<SaleVM>, SalesService>();
builder.Services.AddScoped<IDropDownOptions<EmployeeVM>, EmployeeService>();
builder.Services.AddScoped<IDropDownOptions<GymClassVM>, GymClassService>();

//CHECKER INJECTION
builder.Services.AddScoped<IChecker, CheckerService>();

//CALENDAR INJECTION
builder.Services.AddScoped<IGymclassCalendarService, GymclassCalendarService>();


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
    pattern: "{controller=Account}/{action=Login}/{id?}");


using(var scope = app.Services.CreateScope())
{
    var initializationService = scope.ServiceProvider.GetRequiredService<IBusinessReportService>();
    initializationService.AddService(); //run to add new business report if new month begins
    initializationService.UpdateService();
}

using (var scope = app.Services.CreateScope())
{
    var initializationService = scope.ServiceProvider.GetRequiredService<IChecker>();
    initializationService.ClientCheck(); //check everytime program starts
}

//for seeding intial data to program only
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

app.Run();
