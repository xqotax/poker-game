using AvtMedia.CleanArchitecture.InfrastructureLayer.Extensions.Outbox;
using Domain.Common;
using Domain.Games.Repositories;
using Domain.Users.Repositories;
using Infrastructure;
using Infrastructure.Repository;

namespace GrpcApp.Configuration;

public class RepositoryInstaller : IServiceInstaller
{
	public void Install(IServiceCollection services, IConfiguration configuration)
	{
		services.AddScoped<OutboxMessagesRepository<ApplicationDbContext>>();
		services.AddScoped<IUsersRepository, UsersRepository>();
		services.AddScoped<IGamesRepository, GamesRepository>();

		services.AddScoped<IUnitOfWork, UnitOfWork>();
	}
}
