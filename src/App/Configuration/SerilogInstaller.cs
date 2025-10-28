using Serilog.Formatting.Compact;
using Serilog;

namespace GrpcApp.Configuration;

public class SerilogInstaller : IServiceInstaller
{
	public void Install(IServiceCollection services, IConfiguration configuration)
	{
		var loggerConfiguration = new LoggerConfiguration()
			.ReadFrom.Configuration(configuration)
			.Enrich.With(new AvtMedia.Serilog.Extensions.Enricher.RemovePropertiesEnricher())
			.WriteTo.Console(new RenderedCompactJsonFormatter());

		Log.Logger = loggerConfiguration.CreateLogger();

		services.AddSerilog();
	}
}
