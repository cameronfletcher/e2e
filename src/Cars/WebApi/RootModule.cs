namespace Cars.WebApi
{
    using System;
    using System.Reflection;
    using Nancy;

    [CLSCompliant(false)]
    public sealed class RootModule : NancyModule
    {
        public RootModule()
        {
            this.Get["/"] = _ =>
            {
                var assembly = typeof(Program).Assembly;

                var title = assembly.GetFirstOrDefault<AssemblyTitleAttribute>();
                var product = assembly.GetFirstOrDefault<AssemblyProductAttribute>();
                var version = assembly.GetFirstOrDefault<AssemblyInformationalVersionAttribute>();

                return new
                {
                    Name = $"{title?.Title ?? product?.Product}",
                    Version = $"{version?.InformationalVersion ?? assembly.GetName().Version.ToString()}",
                    CarsUrl = new Uri(this.Request.Url).Combine("cars").ToString(),
                };
            };
        }
    }
}
