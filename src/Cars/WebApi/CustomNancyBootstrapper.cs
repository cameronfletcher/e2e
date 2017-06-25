namespace Cars.WebApi
{
    using System;
    using Autofac;
    using Cars.Model;
    using Cars.Persistence;
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Bootstrappers.Autofac;
    using Nancy.Responses.Negotiation;

    [CLSCompliant(false)]
    public sealed class CustomNancyBootstrapper : AutofacNancyBootstrapper
    {
        private readonly ILifetimeScope lifetimeScope;

        public CustomNancyBootstrapper(ILifetimeScope lifetimeScope)
        {
            Guard.Against.Null(() => lifetimeScope);

            this.lifetimeScope = lifetimeScope;
        }

        protected override NancyInternalConfiguration InternalConfiguration =>
            NancyInternalConfiguration.WithOverrides(builder => builder.ResponseProcessors = new[] { typeof( JsonProcessor) });

        protected override ILifetimeScope GetApplicationContainer() => this.lifetimeScope;

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            pipelines.OnError.AddItemToEndOfPipeline((ctx, ex) =>
            {
                ctx.Items.Add("handled_message_detail", ex.Message);

                switch (ex)
                {
                    case BusinessException error:
                        return HttpStatusCode.BadRequest;

                    case ConcurrencyException error:
                        return HttpStatusCode.Conflict;

                    case AggregateRootNotFoundException error:
                        return HttpStatusCode.NotFound;

                    default:
                        return HttpStatusCode.InternalServerError;
                }
            });

            base.RequestStartup(container, pipelines, context);
        }
    }
}
