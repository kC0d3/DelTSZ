using DelTSZ.Data;
using DelTSZ.Models.Enums;
using DelTSZ.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace DelTSZ.Repositories.UserRepository;

public class UserRepository(DataContext dataContext) : IUserRepository
{
    public async Task<User?> GetOwner()
    {
        return await dataContext.Users.FirstOrDefaultAsync(u => u.Role == Roles.Owner.ToString());
    }
}