using DelTSZ.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = DbConnection.GetDockerConnectionString();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
AddDbContext();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

//Application methods

void AddDbContext()
{
    builder.Services.AddDbContext<DataContext>(optionsBuilder =>
        optionsBuilder.UseSqlServer(connectionString));
}
