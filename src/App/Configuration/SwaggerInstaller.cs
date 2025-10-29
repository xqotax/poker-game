using Microsoft.OpenApi.Models;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace App.Configuration;

public class SwaggerInstaller : IServiceInstaller
{
	public void Install(IServiceCollection services, IConfiguration configuration)
	{
		var version = "v1";

		services.AddSwaggerGen(setup =>
		{
			setup.SwaggerDoc(version, new OpenApiInfo
			{
				Description = "",
				Title = "Poker game",
				Version = version,
			});

			setup.AddEnumsWithValuesFixFilters();
		});
	}
}
