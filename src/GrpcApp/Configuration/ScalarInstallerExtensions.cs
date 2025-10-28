namespace App.Configuration;
public static class ScalarInstallerExtensions
{
	public static IEndpointConventionBuilder MapScalarUi(this IEndpointRouteBuilder endpoints)
		=> endpoints.MapGet("/scalar/{swaggerVersion}", (string swaggerVersion) => Results.Content($$"""
              <!doctype html>
              <html>
              <head>
                  <title>Scalar API Reference -- Swagger {{swaggerVersion}}</title>
                  <meta charset="utf-8" />
                  <meta
                  name="viewport"
                  content="width=device-width, initial-scale=1" />
              </head>
              <body>
                  <script
                  id="api-reference"
                  data-url="/swagger/{{swaggerVersion}}/swagger.json"></script>
                  <script>
                  var configuration = {
                      theme: 'purple',
                  }
              
                  document.getElementById('api-reference').dataset.configuration =
                      JSON.stringify(configuration)
                  </script>
                  <script src="https://cdn.jsdelivr.net/npm/@scalar/api-reference"></script>
                 <style> .absolute-link-div {position: fixed;bottom: 10px;right: 10px;background-color:grey;padding: 3px;}
                 .absolute-link {
                 position: revative;
                 color: white;
                 text-decoration: none;
                 font-size: 24px;
                 font-weight: 700;
                 }</style><div class="absolute-link-div"><a href="/swagger/{{swaggerVersion}}/swagger.json" class="absolute-link">Swagger json</a></div>
              </body>
              </html>
              """, "text/html")).ExcludeFromDescription();
}
