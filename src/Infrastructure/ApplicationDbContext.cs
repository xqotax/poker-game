using AvtMedia.CleanArchitecture.InfrastructureLayer.Extensions.Constants;
using AvtMedia.CleanArchitecture.InfrastructureLayer.Extensions.DbQueryLogger.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public sealed class ApplicationDbContext : DbContext
{
	public const string SchemaName = "poker-game";

	private readonly IDbQueryLogger _dbQueryLogger;

	public ApplicationDbContext(DbContextOptions options, IDbQueryLogger dbQueryLogger)
		: base(options) => _dbQueryLogger = dbQueryLogger;

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		_dbQueryLogger.AddDbQueryLogger(optionsBuilder, [SchemaName + $".\"{OutboxMessageTableName.OutboxMessage}\""]);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.HasDefaultSchema(SchemaName);

		modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
	}
}
