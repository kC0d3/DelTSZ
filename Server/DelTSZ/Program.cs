using DelTSZ.Data;
using DelTSZ.Models.Addresses;
using DelTSZ.Models.Users;
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

app.Run();
//Application methods

void AddServices()
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

void AddDbContext()
{
    builder.Services.AddDbContext<DataContext>(optionsBuilder =>
        optionsBuilder.UseSqlServer(connectionString));
}

void AddIdentity()
{
    builder.Services
        .AddIdentityCore<User>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<DataContext>();
}

void AddRoles()
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var tOwner = CreateOwnerRole(roleManager);
    tOwner.Wait();

    var tProducer = CreateProducerRole(roleManager);
    tProducer.Wait();

    var tCostumer = CreateCostumerRole(roleManager);
    tCostumer.Wait();
}

async Task CreateOwnerRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("Owner"));
}

async Task CreateProducerRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("Producer"));
}

async Task CreateCostumerRole(RoleManager<IdentityRole> roleManager)
{
    await roleManager.CreateAsync(new IdentityRole("Costumer"));
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
            UserName = "DelTSZ", Email = "deltsz@deltsz.com", CompanyName = "DelTSZ", Role = "Owner",
            Address = new Address { ZipCode = "6600", City = "Szentes", Street = "Szarvasi út", HouseNumber = "3" }
        };
        var ownerCreated = await userManager.CreateAsync(owner, "DelTSZ!123");

        if (ownerCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(owner, "Owner");
        }
    }
}