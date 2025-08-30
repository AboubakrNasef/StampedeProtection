using Microsoft.Extensions.Caching.Memory;
using StampedeProtection.Api.Shared.Application;
using StampedeProtection.Api.Shared.Application.Repositories;
using StampedeProtection.Api.Shared.Domain;

namespace StampedeProtection.Api.Features
{
    public static class GetProductWithMemoryEndPoint
    {
        public static void AddGetProductWithMemoryEndPoint(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/products-memory-cache/{id:int}", async (int id, GetProductWithMemoryRequestHandler handler) =>
            {
                var product = await handler.Handle(id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            });
        }
    }

    public sealed class GetProductWithMemoryRequestHandler : RequestHandler<int, Product>
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<GetProductWithMemoryRequestHandler> _logger;
        private readonly IMemoryCache _memoryCache;
        public GetProductWithMemoryRequestHandler(IProductRepository productRepository, ILogger<GetProductWithMemoryRequestHandler> logger, IMemoryCache memoryCache)
        {
            _productRepository = productRepository;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<Product> Handle(int id)
        {
            if (_memoryCache.TryGetValue<Product>(id,out var cachedProduct))
            {
                _logger.LogInformation("returning Cached Product {Id} From Repository", id);
                return cachedProduct;
            }
            _logger.LogInformation("Getting Product {Id} From Repository", id);
            var product = await _productRepository.GetByIdAsync(id);
            _memoryCache.Set(id, product, TimeSpan.FromMinutes(5));
            return product;
        }
    }
}
