namespace Cars.WebApi
{
    using System;
    using System.Reflection;
    using Microsoft.Owin.Hosting;
    using Nancy.Bootstrapper;
    using Owin;

    [CLSCompliant(false)]
    public sealed class NancyWebApi : IDisposable
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string apiUrl;
        private readonly INancyBootstrapper bootstrapper;

        private IDisposable host;

#pragma warning disable S3994 // NOTE (Cameron): Represents an HTTP URL namespace and may not always be represented as a URI.
        public NancyWebApi(string apiUrl, INancyBootstrapper bootstrapper)
        {
            Guard.Against.Null(() => apiUrl);
            Guard.Against.Null(() => bootstrapper);

            this.apiUrl = apiUrl;
            this.bootstrapper = bootstrapper;
        }

        public void Start()
        {
            if (this.host != null)
            {
                throw new InvalidOperationException($"The web API service has already running on '{this.apiUrl}'!");
            }

            this.host = WebApp.Start(
                this.apiUrl,
                app =>
                {
                    app.UseNancy(o => o.Bootstrapper = this.bootstrapper);
                });

            Log.Info($"The web API service has started listening on '{this.apiUrl}'.");
        }

        public void Stop()
        {
            if (this.host == null)
            {
                return;
            }

            this.Dispose();

            Log.Info($"The web API service has stopped listening on '{this.apiUrl}'.");
        }

        public void Dispose()
        {
            if (this.host == null)
            {
                return;
            }

            this.host.Dispose();
            this.host = null;
        }
    }
}
