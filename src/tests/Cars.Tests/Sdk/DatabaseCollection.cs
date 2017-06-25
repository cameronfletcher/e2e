namespace Cars.Tests.Sdk
{
    using Xunit;

    /*  LINK (Cameron): http://xunit.github.io/docs/shared-context.html
        These classes have no code, and are never created. Their purpose is simply
        to be the place to apply [CollectionDefinition] and all the
        ICollectionFixture<> interfaces.  */

    [CollectionDefinition("SQL Server Collection")]
    public class DatabaseCollection : ICollectionFixture<SqlServerFixture>
    {
    }
}
