namespace StampedeProtection.Api.Shared.Application
{
    public interface RequestHandler<TRequest, TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
