using System;
using System.Linq;

namespace NancyFX.BeforeResponse
{
    using Nancy;
    using Nancy.Configuration;
    using Nancy.Responses;
    using Nancy.Responses.Negotiation;
    using Nancy.Validation;

    public class ErrorResponse : JsonResponse<Error>
    {
        public ErrorResponse(Error model, INancyEnvironment environment) : base(model, new DefaultJsonSerializer(environment), environment)
        {
            StatusCode = model.StatusCode;
        }
    }

    public static class NancyErrorResponseExtensions
    {
        public const int ErrorCodeException = 0;
        public const int ErrorCodeMessage = 1;
        public const int ErrorCodeValidation = 2;

        public static Negotiator WithError(this Negotiator negotiator, Exception exception)
        {
            return negotiator
                .WithModel(new Error() { Code = ErrorCodeException, Message = exception.Message, Details = exception.ToString(), StatusCode = HttpStatusCode.InternalServerError })
                .WithStatusCode(HttpStatusCode.InternalServerError);
        }

        public static Negotiator WithError(this Negotiator negotiator, string errorMessage)
        {
            return negotiator
                .WithModel(new Error() { Code = ErrorCodeException, Message = errorMessage, StatusCode = HttpStatusCode.InternalServerError })
                .WithStatusCode(HttpStatusCode.InternalServerError);
        }

        public static Negotiator WithError(this Negotiator negotiator, string errorMessage, HttpStatusCode statusCode)
        {
            return negotiator
                .WithModel(new Error() { Code = ErrorCodeException, Message = errorMessage, StatusCode = statusCode })
                .WithStatusCode(statusCode);
        }

        public static Negotiator WithError(this Negotiator negotiator, Error error)
        {
            return negotiator
                .WithModel(error)
                .WithStatusCode(error.StatusCode);
        }

        public static Error ToError(this ModelValidationResult modelValidationResult)
        {
            var errors =
                   Enumerable.ToArray<ValidationErrors>((from error in modelValidationResult.Errors
                                                         select new ValidationErrors { Field = error.Key, Errors = Enumerable.ToArray<string>(error.Value.Select(x => x.ErrorMessage)) }));

            return new Error()
            {
                Code = ErrorCodeValidation,
                Message = String.Join(" ", errors.SelectMany(x => x.Errors)),
                ValidationErrors = errors
            };
        }

        public static Negotiator WithModelValidationError(this Negotiator negotiator, ModelValidationResult modelValidationResult)
        {
            return negotiator
                .WithModel(ToError(modelValidationResult))
                .WithStatusCode(HttpStatusCode.InternalServerError);
        }
    }

    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
        public ValidationErrors[] ValidationErrors { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public class ValidationErrors
    {
        public string Field { get; set; }
        public string[] Errors { get; set; }
    }
}