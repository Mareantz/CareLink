﻿using Domain.Common;
using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<Result<Guid>> AddUser(User user);
        Task<IEnumerable<User>> GetUsers();
        Task UpdateUser(User user);
        Task<Result<Guid>> Register(User user, CancellationToken cancellationToken);
        Task<Result<string>> Login(string username, string password);
    }
}
