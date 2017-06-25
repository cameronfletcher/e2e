namespace Cars.Tests.Sdk
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;

    public sealed class WebApiFixture : IDisposable
    {
        private readonly Uri apiUrl = new Uri(ConfigurationManager.AppSettings["ApiUrl"]);

        public WebApiFixture()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                var serverConnection = new ServerConnection(connection);
                var server = new Server(serverConnection);
                var database = new Database(server, this.databaseName);

                database.Create();
            }
        }

        public Uri ApiUrl
        {
            get { return this.apiUrl; }
        }

        public void Dispose()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                var serverConnection = new ServerConnection(connection);
                var server = new Server(serverConnection);

                server.KillDatabase(this.databaseName);
            }
        }
    }
}
