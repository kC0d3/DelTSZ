﻿using DelTSZ.Models.Users;

namespace DelTSZ.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User?> GetOwner();
    Task<UserResponse?> GetUserAllDataById(string id);
    Task<IEnumerable<UserResponse>> GetProducers();
    Task<IEnumerable<UserResponse>> GetCostumers();
}