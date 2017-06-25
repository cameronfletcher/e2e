namespace Cars.WebApi
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Nancy;
    using Nancy.ErrorHandling;
    using Nancy.Responses;
    using Nancy.Serialization.JsonNet;
    using Newtonsoft.Json;

    [CLSCompliant(false)]
    public class StatusCodeHandler : IStatusCodeHandler
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly HashSet<HttpStatusCode> HandledStatusCodes =
            new HashSet<HttpStatusCode> { HttpStatusCode.InternalServerError, HttpStatusCode.NotFound, HttpStatusCode.BadRequest };

        private readonly ISerializer serializer;

        public StatusCodeHandler(JsonSerializer serializer)
        {
            this.serializer = new JsonNetSerializer(serializer);
        }

#pragma warning disable S1541 // NOTE (Cameron): Probably deserves some code review.
        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            var model = default(object);
            context.Items.TryGetValue("handled_message_detail", out var messageDetail);

#pragma warning disable S131 // NOTE (Cameron): Not required by Nancy runtime; see HandleStatusCode method (below).
            switch (statusCode)
            {
                case HttpStatusCode.InternalServerError:
                    var reference = Guid.NewGuid().ToString("N");
                    context.Items.TryGetValue(NancyEngine.ERROR_EXCEPTION, out var value);
                    var exception = value as RequestExecutionException;
                    Log.Error($"Unhandled error ref: '{reference}'.", exception?.InnerException);
                    model = new
                    {
                        Message = "Internal Server Error",
                        Reference = reference,
                        Summary = exception?.InnerException?.Message,
                        Details = exception?.InnerException?.ToString(),
                    };
                    break;

                case HttpStatusCode.NotFound:
                    model = new
                    {
                        Message = "Not Found",
                        Details = messageDetail?.ToString(),
                    };
                    break;

                case HttpStatusCode.Conflict:
                    model = new
                    {
                        Message = "Conflict",
                        Details = messageDetail?.ToString(),
                    };
                    break;

                case HttpStatusCode.BadRequest:
                    model = new
                    {
                        Message = "Bad Request",
                        Details = messageDetail?.ToString(),
                    };
                    break;
            }

            context.Response = new JsonResponse(model, this.serializer) { StatusCode = statusCode };
        }

        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return HandledStatusCodes.Contains(statusCode);
        }
    }
}
