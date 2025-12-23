using AutoBackend.Sdk.Exceptions.Api;
using AutoBackend.Sdk.Exceptions.Data;

namespace AutoBackend.Sdk.Extensions;

internal static class ExceptionExtensions
{
    extension(Exception exception)
    {
        internal ApiException ToApiException()
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
                    UnauthorizedAccessDataException => new UnauthorizedApiException(
                        exception.Message,
                        exception),
                    _ => fallbackValue
                },
                _ => fallbackValue
            };
        }
    }
}