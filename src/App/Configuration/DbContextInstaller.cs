using AvtMedia.CleanArchitecture.InfrastructureLayer.Extensions.DbQueryLogger.Interfaces;
using AvtMedia.CleanArchitecture.InfrastructureLayer.Extensions.DbQueryLogger;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrpcApp.Configuration;

public class DbContextInstaller : IServiceInstaller
{
	public void Install(IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<ApplicationDbContext>((sp, optionsBuilder) =>
		{
			string connectionString = configuration.GetConnectionString("Database")!;

			optionsBuilder.UseNpgsql(connectionString,
				o => o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, ApplicationDbContext.SchemaName));
		});

		services.AddScoped<IDbQueryLogger, DbQueryLogger>();
	}
}
