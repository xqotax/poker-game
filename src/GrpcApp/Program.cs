using App.Configuration;
using App.Middlewares;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

builder.Configuration.AddEnvironmentVariables();

builder.Services.InstallServices(builder.Configuration, typeof(IServiceInstaller).Assembly, typeof(Program).Assembly);

builder.Services.AddScoped<GlobalErrorHandlerMiddleware>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseRouting();
app.UseCors();

app.UseHttpsRedirection();

app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.Use(async (context, next) =>
{
	context.Response.Headers.Append("Cross-Origin-Opener-Policy", "unsafe-none");
	context.Response.Headers.Append("Cross-Origin-Embedder-Policy", "unsafe-none");

	if (context.Request.Path == "/")
	{
		context.Response.Redirect("/scalar/v1", permanent: true);
		return;
	}

	await next();
});
app.UseSwagger();
app.UseSwaggerUI(setup =>
{
	setup.SwaggerEndpoint("/swagger/v1/swagger.json", "AVT Media API v1");
	setup.RoutePrefix = "openapi";
});
app.MapScalarUi();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	db.Database.Migrate();
}

await app.RunAsync();
