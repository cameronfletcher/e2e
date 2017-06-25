namespace Cars.Tests.Sdk
{
    using System.Data.SqlClient;
    using Microsoft.SqlServer.Management.Common;
    using Xunit;

    public static class Integration
    {
        public abstract class Database : IClassFixture<SqlServerFixture>
        {
            protected Database(SqlServerFixture fixture)
            {
                this.ConnectionString = fixture.ConnectionString.Replace("=master;", string.Concat("=", fixture.DatabaseName, ";"));
            }

            protected string ConnectionString { get; set; }

            protected void ExecuteScript(string sqlScript)
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    new ServerConnection(connection).ExecuteNonQuery(sqlScript);
                }
            }
        }

        public abstract class WebApi
        {

        }
    }
}
