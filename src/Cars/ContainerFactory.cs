namespace Cars
{
    using Autofac;
    using Cars.Persistence;
    using Cars.Persistence.SqlServer;
    using Cars.WebApi;
    using Newtonsoft.Json;

    internal static class ContainerFactory
    {
#pragma warning disable S3994 // NOTE (Cameron): Represents an HTTP URL namespace and may not always be represented as a URI.
        public static IContainer Build(string connectionString, string apiUrl)
        {
            var builder = new ContainerBuilder();

            builder.Register(context => new CarRepository(connectionString)).As<ICarRepository>().SingleInstance();
            builder.Register(context => new NancyWebApi(apiUrl, new CustomNancyBootstrapper(context.Resolve<ILifetimeScope>()))).AsSelf().SingleInstance();
            builder.RegisterType<CustomJsonSerializer>().As<JsonSerializer>();
            builder.RegisterInstance(typeof(ContainerFactory).Assembly);

            return builder.Build();
        }
    }
}
