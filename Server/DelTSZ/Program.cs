using DelTSZ.Data;
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

AddRoles();

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