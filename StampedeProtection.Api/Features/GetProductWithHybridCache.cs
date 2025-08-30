using Microsoft.Extensions.Caching.Hybrid;
using StampedeProtection.Api.Shared.Application;
using StampedeProtection.Api.Shared.Application.Repositories;
using StampedeProtection.Api.Shared.Domain;

namespace StampedeProtection.Api.Features
{
    public static class GetProductWithHybridEndPoint
    {
        public static void AddGetProductWithHybridEndPoint(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/products-hybrid-cache/{id:int}", async (int id, GetProductWithHybridRequestHandler handler) =>
            {
                var product = await handler.Handle(id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });
        }
    }

    public sealed class GetProductWithHybridRequestHandler : RequestHandler<int, Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetProductWithHybridRequestHandler> _logger;
        private readonly HybridCache _HybridCache;
        public GetProductWithHybridRequestHandler(IProductRepository productRepository, ILogger<GetProductWithHybridRequestHandler> logger, HybridCache HybridCache)
        {
            _productRepository = productRepository;
            _logger = logger;
            _HybridCache = HybridCache;
        }

        public async Task<Product> Handle(int id)
        {
            return await _HybridCache.GetOrCreateAsync<Product>(id.ToString(), async (cancel) =>
               {
                   _logger.LogInformation("Getting Product {Id} From Repository", id);
                   return await _productRepository.GetByIdAsync(id);
               });
     
        }
    }
}
