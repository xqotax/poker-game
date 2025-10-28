using Microsoft.OpenApi.Models;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace GrpcApp.Configuration;

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
				Title = "AVT Media",
				Version = version,
			});

			setup.AddEnumsWithValuesFixFilters();
		});
	}
}
