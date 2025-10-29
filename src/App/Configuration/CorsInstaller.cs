namespace App.Configuration;

public class CorsInstaller : IServiceInstaller
{

	public void Install(IServiceCollection services, IConfiguration configuration) => 
		services.AddCors(opt => opt.AddDefaultPolicy(corsBuilder =>
		{
			var origins = GetOrigins(configuration);

			corsBuilder
				.WithOrigins([.. origins])
				.AllowAnyMethod()
				.AllowAnyHeader()
				.AllowCredentials();
		}));


	public static List<string> GetOrigins(IConfiguration configuration)
	{
		List<string> origins = [];
		int i = 0;
		while (true)
		{
			var address = configuration[$"CorsAllowList:Addresses:{i}"];

			if (address == null)
				break;

			origins.Add(address.Trim());

			i++;
		}

		return origins;	
	}
}


