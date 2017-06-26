namespace Cars.Website
{
    using System.IO;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    internal static class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseUrls("http://+:80/e2e/web/")
                .ConfigureLogging(loggerFactory => loggerFactory.AddConsole())
                .Configure(
                    app =>
                    {
                        app.UseDefaultFiles("/e2e/web");
                        app.UseStaticFiles("/e2e/web");
                    })
                .Build();

            host.Run();
        }
    }
}