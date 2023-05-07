using AutoBackend.Sdk.Exceptions.Api;
using AutoBackend.Sdk.Exceptions.Configuration;
using AutoBackend.Sdk.Exceptions.Data;
using AutoBackend.Sdk.Exceptions.Reflection;

namespace AutoBackend.Sdk.Extensions;

internal static class ExceptionExtensions
{
    internal static ApiException ToApiException(this Exception exception)
    {
        var fallbackValue = new InternalServerErrorApiException(
            Constants.AnUnexpectedInternalServerErrorHasHappened,
            exception);
        return exception switch
        {
            ApiException apiException => apiException,
            TaskCanceledException => new TaskCanceledApiException(
                Constants.TheOperationHasBeenCanceled,
                exception),
            DataException dataException => dataException switch
            {
                InconsistentDataException => new BadRequestApiException(
                    Constants.InconsistentDataHasBeenFound,
                    exception),
                NotFoundDataException => new NotFoundApiException(
                    Constants.NoDataHasBeenFound,
                    exception),
                _ => fallbackValue
            },
            ReflectionException => fallbackValue,
            ConfigurationException => fallbackValue,
            _ => fallbackValue
        };
    }
}