namespace Cars.WebApi.WebApi
{
    using System;
    using System.Linq;
    using Cars.Model;
    using Cars.Persistence;
    using Nancy;
    using Nancy.ModelBinding;

    [CLSCompliant(false)]
    public sealed class CarsModule : NancyModule
    {
        public CarsModule(ICarRepository repoisitory)
            : base("api/cars")
        {
            Guard.Against.Null(() => repoisitory);

            this.Get["/", runAsync: true] = async (paramaters, token) =>
                (await repoisitory.GetCars()).Select(car =>
                    new
                    {
                        url = new Uri(this.Request.Url).Combine(car.registration).ToString(),
                        car.registration,
                        car.totalDistanceTravelled,
                    });

            this.Post["/", runAsync: true] = async (paramaters, token) =>
            {
                var body = this.Bind<Body>();
                var car = new Car(body.Registration);
                await repoisitory.Save(car);
                return new Response { StatusCode = HttpStatusCode.Created }.WithHeader("Location", new Uri(this.Request.Url).Combine(body.Registration).ToString());
            };

            this.Get["/{Registration}/", runAsync: true] = async (paramaters, token) =>
            {
                var car = await repoisitory.GetCar((string)paramaters.Registration);
                if (car == null)
                {
                    // NOTE (Cameron): Placed here to mimic the response from the handling of the AggregateRootNotFound exception thrown by the non-read-model repository.
                    this.Context.Items.Add("handled_message_detail", $"Cannot find any car with registration '{(string)paramaters.Registration}'.");
                    return HttpStatusCode.NotFound;
                }

                return new
                {
                    Url = new Uri(this.Request.Url).Sanitize().ToString(),
                    CarsUrl = new Uri(this.Request.Url.SiteBase).Combine(this.Request.Url.BasePath, "api/cars").ToString(),
                    car.Value.registration,
                    car.Value.totalDistanceTravelled,
                };
            };

            this.Patch["/{Registration}/", runAsync: true] = async (paramaters, token) =>
            {
                var body = this.Bind<Body>();
                var car = await repoisitory.Load((string)paramaters.Registration);
                car.Drive(body.DistanceTravelled);
                await repoisitory.Save(car);
                return HttpStatusCode.OK;
            };

            // LINK (Cameron): https://stackoverflow.com/questions/6439416/deleting-a-resource-using-http-delete
            this.Delete["/{Registration}/", runAsync: true] = async (paramaters, token) =>
            {
                var car = await repoisitory.Load((string)paramaters.Registration);
                car.Scrap();
                await repoisitory.Save(car);
                return HttpStatusCode.OK;
            };
        }

#pragma warning disable S1144, S3459
        private class Body
        {
            public string Registration { get; set; }

            public int DistanceTravelled { get; set; }
        }
    }
}
