using DelTSZ.Data;
using DelTSZ.Models.Addresses;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Ingredients;
using DelTSZ.Models.Products;
using DelTSZ.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.UserRepository;

public class UserRepository(DataContext dataContext) : IUserRepository
{
    public async Task<User?> GetOwner()
    {
        return await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
    }

    public async Task<UserResponse?> GetUserAllDataById(string id)
    {
        return await dataContext.Users
            .Include(u => u.Ingredients)
            .Include(u => u.Products)
            .Include(u => u.Address)
            .Where(u => u.Id == id)
            .Select(u => new UserResponse
            {
                Email = u.Email,
                UserName = u.UserName,
                CompanyName = u.CompanyName,
                Role = u.Role,
                Address = new AddressResponse
                {
                    ZipCode = u.Address!.ZipCode,
                    City = u.Address.City,
                    Street = u.Address.Street,
                    HouseNumber = u.Address.HouseNumber
                },
                Ingredients = u.Ingredients!.Select(i => new IngredientResponse
                {
                    Id = i.Id,
                    Type = i.Type,
                    Received = i.Received,
                    Amount = i.Amount
                }).ToList(),
                Products = u.Products!.Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Type = p.Type,
                    Packed = p.Packed,
                    Amount = p.Amount
                }).ToList()
            }).FirstOrDefaultAsync();
    }
    
    public async Task<User?> GetUserWithAddressById(string id)
    {
        return await dataContext.Users
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<IEnumerable<UserResponse>> GetProducers()
    {
        return await dataContext.Users
            .Include(u => u.Ingredients)
            .Include(u => u.Address)
            .Where(u => u.Role == Roles.Producer.ToString())
            .Select(u => new UserResponse
            {
                Email = u.Email,
                UserName = u.UserName,
                CompanyName = u.CompanyName,
                Role = u.Role,
                Address = new AddressResponse
                {
                    ZipCode = u.Address!.ZipCode,
                    City = u.Address.City,
                    Street = u.Address.Street,
                    HouseNumber = u.Address.HouseNumber
                },
                Ingredients = u.Ingredients!.Select(i => new IngredientResponse
                {
                    Id = i.Id,
                    Type = i.Type,
                    Received = i.Received,
                    Amount = i.Amount
                }).ToList(),
            }).ToListAsync();
    }

    public async Task<IEnumerable<UserResponse>> GetCostumers()
    {
        return await dataContext.Users
            .Include(u => u.Ingredients)
            .Include(u => u.Address)
            .Where(u => u.Role == Roles.Costumer.ToString())
            .Select(u => new UserResponse
            {
                Email = u.Email,
                UserName = u.UserName,
                CompanyName = u.CompanyName,
                Role = u.Role,
                Address = new AddressResponse
                {
                    ZipCode = u.Address!.ZipCode,
                    City = u.Address.City,
                    Street = u.Address.Street,
                    HouseNumber = u.Address.HouseNumber
                },
                Ingredients = u.Ingredients!.Select(i => new IngredientResponse
                {
                    Id = i.Id,
                    Type = i.Type,
                    Received = i.Received,
                    Amount = i.Amount
                }).ToList(),
            }).ToListAsync();
    }
}