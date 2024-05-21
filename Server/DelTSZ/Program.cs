using DelTSZ.Data;
using DelTSZ.Models.Addresses;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Users;
using DelTSZ.Repositories.IngredientRepository;
using DelTSZ.Repositories.ProductRepository;
using DelTSZ.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = DbConnection.GetDockerConnectionString();

AddDbContext();
AddServices();
AddIdentity();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

await app.Services.InitializeDbAsync();
AddRoles();
AddOwner();
AddProducer();
AddCostumer();

app.Run();

//Application methods

void AddServices()
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddIdentityApiEndpoints<User>();
    builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
}

void AddDbContext()
{
    builder.Services.AddDbContext<DataContext>(optionsBuilder =>
        optionsBuilder.UseSqlServer(connectionString));
}

void AddIdentity()
{
    builder.Services
        .ConfigureApplicationCookie(options => options.ExpireTimeSpan = TimeSpan.FromMinutes(10))
        .AddIdentityCore<User>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<DataContext>();
}

void AddRoles()
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (roleManager.Roles.FirstOrDefault(r => r.Name == Roles.Owner.ToString()) == null)
    {
        foreach (Roles role in Enum.GetValues(typeof(Roles)))
        {
            var tRole = CreateRole(roleManager, role);
            tRole.Wait();
        }
    }
}

async Task CreateRole(RoleManager<IdentityRole> roleManager, Roles role)
{
    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
}

void AddOwner()
{
    var tOwner = CreateOwnerIfNotExists();
    tOwner.Wait();
}

async Task CreateOwnerIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var ownerInDb = await userManager.FindByEmailAsync("deltsz@deltsz.com");
    if (ownerInDb == null)
    {
        var owner = new User
        {
            UserName = "DelTSZ", Email = "deltsz@deltsz.com", CompanyName = "DelTSZ", Role = Roles.Owner.ToString(),
            Address = new Address { ZipCode = "6600", City = "Szentes", Street = "Szarvasi út", HouseNumber = "3" }
        };
        var ownerCreated = await userManager.CreateAsync(owner, "DelTSZ!123");

        if (ownerCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(owner, Roles.Owner.ToString());
        }
    }
}

void AddProducer()
{
    var tProducer = CreateProducerIfNotExists();
    tProducer.Wait();
}

async Task CreateProducerIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var producerInDb = await userManager.FindByEmailAsync("arpad@arpad.com");
    if (producerInDb == null)
    {
        var producer = new User
        {
            UserName = "Arpad", Email = "arpad@arpad.com", CompanyName = "Árpád Zrt.", Role = Roles.Producer.ToString(),
            Address = new Address { ZipCode = "6600", City = "Szentes", Street = "Apponyi tér", HouseNumber = "12" }
        };
        var producerCreated = await userManager.CreateAsync(producer, "Arpad!123");

        if (producerCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(producer, Roles.Producer.ToString());
        }
    }
}

void AddCostumer()
{
    var tCostumer = CreateCostumerIfNotExists();
    tCostumer.Wait();
}

async Task CreateCostumerIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var costumerInDb = await userManager.FindByEmailAsync("aldi@aldi.com");
    if (costumerInDb == null)
    {
        var costumer = new User
        {
            UserName = "ALDI", Email = "aldi@aldi.com", CompanyName = "ALDI Magyarország Élelmiszer Bt.",
            Role = Roles.Costumer.ToString(),
            Address = new Address
                { ZipCode = "2051", City = "Biatorbágy", Street = "Mészárosok útja", HouseNumber = "2" }
        };
        var costumerCreated = await userManager.CreateAsync(costumer, "Aldi!123");

        if (costumerCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(costumer, Roles.Costumer.ToString());
        }
    }
}