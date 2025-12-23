namespace AutoBackend.Sdk.Services.ExceptionHandler;

internal interface IApiExceptionHandler
{
    Task HandleAsync(Exception exception, CancellationToken cancellationToken);
}