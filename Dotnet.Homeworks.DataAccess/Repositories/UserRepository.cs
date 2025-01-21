using Dotnet.Homeworks.Data.DatabaseContext;
using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IQueryable<User>> GetUsersAsync(CancellationToken cancellationToken)
        => _dbContext.Users.AsQueryable();

    public async Task<User?> GetUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
        => await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == guid, cancellationToken);

    public Task DeleteUserByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        var user = _dbContext.Users.FirstOrDefault(x => x.Id == guid);
        
        if (user == null)
            throw new ArgumentNullException(nameof(user));
        
        _dbContext.Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
         _dbContext.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> InsertUserAsync(User user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}