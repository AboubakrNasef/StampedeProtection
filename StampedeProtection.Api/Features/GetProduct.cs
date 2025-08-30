using StampedeProtection.Api.Shared.Application;
using StampedeProtection.Api.Shared.Application.Repositories;
using StampedeProtection.Api.Shared.Domain;

namespace StampedeProtection.Api.Features
{
    public static class GetProductEndPoint
    {
        public static void AddGetProductEndPoint(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/products/{id:int}", async (int id, GetProductRequestHandler handler) =>
            {
                var product = await handler.Handle(id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });
        }
    }

    public sealed class GetProductRequestHandler : RequestHandler<int, Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetProductRequestHandler> _logger;

        public GetProductRequestHandler(IProductRepository productRepository, ILogger<GetProductRequestHandler> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<Product> Handle(int id)
        {
            _logger.LogInformation("Geting Product {Id} From Repository", id);
            var product = await _productRepository.GetByIdAsync(id);

            return product;
        }
    }
}
