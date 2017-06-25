namespace Cars.WebApi
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public sealed class CustomJsonSerializer : JsonSerializer
    {
        public CustomJsonSerializer()
        {
            this.ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy(true, false) };
            this.NullValueHandling = NullValueHandling.Ignore;
        }
    }
}
