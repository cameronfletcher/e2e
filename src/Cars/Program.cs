namespace Cars.WebApi
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using Autofac;
    using Topshelf;

    public static class Program
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        internal static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = (Exception)e.ExceptionObject;
                Log.Error(ex);
                ExceptionDispatchInfo.Capture(ex).Throw();
            };

            var assembly = typeof(Program).Assembly;

            var title = assembly.GetFirstOrDefault<AssemblyTitleAttribute>();
            var product = assembly.GetFirstOrDefault<AssemblyProductAttribute>();
            var copyright = assembly.GetFirstOrDefault<AssemblyCopyrightAttribute>();
            var description = assembly.GetFirstOrDefault<AssemblyDescriptionAttribute>();
            var version = assembly.GetFirstOrDefault<AssemblyInformationalVersionAttribute>();

            Log.Info($"{title?.Title ?? product?.Product} [{version?.InformationalVersion ?? assembly.GetName().Version.ToString()}]");
            Log.Info(copyright?.Copyright);

            var connectionString = ConfigurationManager.ConnectionStrings["Cars"].ConnectionString;
            var apiUrl = ConfigurationManager.AppSettings["ApiUrl"];

            using (var container = ContainerFactory.Build(connectionString, apiUrl))
            {
                HostFactory.Run(
                    config =>
                    {
                        config.UseLog4Net();
                        config.SetServiceName(title.Title);
                        config.SetDisplayName(title.Title);
                        config.SetDescription(description.Description);
                        config.Service<NancyWebApi>(service =>
                        {
                            service.ConstructUsing(() => container.Resolve<NancyWebApi>());
                            service.WhenStarted(x => x.Start());
                            service.WhenStopped(x => x.Stop());
                        });
                    });
            }
        }

#pragma warning disable S4018
        internal static T GetFirstOrDefault<T>(this ICustomAttributeProvider provider) => provider.GetCustomAttributes(typeof(T), false).Cast<T>().FirstOrDefault();
    }
}
