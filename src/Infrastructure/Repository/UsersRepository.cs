using Domain.Users;
using Domain.Users.Repositories;
using Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public sealed class UsersRepository : IUsersRepository
{
	private readonly ApplicationDbContext _dbContext;

	public UsersRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

	public async Task Add(User user, CancellationToken cancellationToken)
	{
		await _dbContext.Set<User>().AddAsync(user, cancellationToken);
	}

	public Task<User[]> GetAll(bool tracking, CancellationToken cancellationToken)
	{
		var query = _dbContext.Set<User>().AsQueryable();

		if (tracking is false)
			query = query.AsNoTracking();

		return query.ToArrayAsync(cancellationToken);
	}

	public Task<User?> GetById(Guid id, CancellationToken cancellationToken)
	{
		return _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
	}

	public Task<bool> NameAlreadyExist(UserName userName, Guid? exceptId, CancellationToken cancellationToken)
	{
		var query = _dbContext.Set<User>().AsQueryable();

		if (exceptId.HasValue)
			query = query.Where(u => u.Id != exceptId.Value);

		return query.AnyAsync(u => u.Username == userName, cancellationToken);
	}
}
