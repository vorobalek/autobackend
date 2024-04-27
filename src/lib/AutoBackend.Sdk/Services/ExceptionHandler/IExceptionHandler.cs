namespace AutoBackend.Sdk.Services.ExceptionHandler;

internal interface IExceptionHandler
{
    Task HandleAsync(Exception exception, CancellationToken cancellationToken);
}