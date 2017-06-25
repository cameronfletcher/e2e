﻿namespace Cars.Tests.Sdk
{
    using System.Data.SqlClient;
    using Microsoft.SqlServer.Management.Common;
    using Xunit;

    public static class Integration
    {
        public class Database : IClassFixture<SqlServerFixture>
        {
            public Database(SqlServerFixture fixture)
            {
                this.ConnectionString = fixture.ConnectionString.Replace("=master;", string.Concat("=", fixture.DatabaseName, ";"));
            }

            public string ConnectionString { get; set; }

            public void ExecuteScript(string sqlScript)
            {
                using (var connection = new SqlConnection(this.ConnectionString))
                {
                    new ServerConnection(connection).ExecuteNonQuery(sqlScript);
                }
            }
        }
    }
}
