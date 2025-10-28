using AvtMedia.CleanArchitecture.InfrastructureLayer.Extensions.Extensions;
using Domain.Common;

namespace Infrastructure;

public sealed class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext _dbContext;

	public UnitOfWork(ApplicationDbContext dbContext) => _dbContext = dbContext;

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		_dbContext.DeleteInSoftDeletedWay();

		_dbContext.ConvertDomainExentsToOutboxMessage();

		_dbContext.UpdateAuditableEntity();

		return _dbContext.SaveChangesAsync(cancellationToken);
	}
}
