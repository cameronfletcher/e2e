namespace Cars.Website
{
    using System;
    using Nancy;
    using Nancy.Responses;

    [CLSCompliant(false)]
    public sealed class WebsiteModule : NancyModule
    {
        public WebsiteModule()
        {
            this.Get["/"] = _ => File("index.html");
            this.Get["/cars/"] = _ => File("cars.html");
            this.Get["/cars/{Registration}/"] = _ => File("car.html");
        }

        private static GenericFileResponse File(string filename) => new GenericFileResponse(string.Concat("Website/", filename), "text/html");
    }
}
