# Cache Stampede Protection Demo

This project demonstrates the cache stampede problem and compares different caching strategies in a .NET 8 Web API application.

## 🚀 Overview

Cache stampede (also known as "thundering herd" or "dog-piling") occurs when multiple threads attempt to access a resource simultaneously after a cache miss, potentially overwhelming the underlying data store. This project showcases three different approaches to handling this problem:

1. **No Caching** - Direct database access on every request
2. **Basic Memory Cache** - Simple caching with `IMemoryCache` (vulnerable to stampede)
3. **Hybrid Cache** - Stampede-protected caching using `HybridCache`

## 🏗️ Project Structure

```
StampedeProtection.Api/
├── K6/                          # Load testing scripts
│   ├── script-hybrid-cache.js   # Test script for hybrid cache endpoint
│   └── script-memory-cache.js   # Test script for memory cache endpoint
├── Features/
│   ├── GetProduct.cs            # No caching implementation
│   ├── GetProductWithMemoryCache.cs  # Basic memory cache
│   └── GetProductWithHybridCache.cs  # Hybrid cache with stampede protection
└── Shared/
    └── Infrastructure/
        └── Repos/
            └── ProductRepository.cs  # Data access layer
```

## 🔌 API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/products/{id}` | GET | Direct database access (no caching) |
| `/products-memory-cache/{id}` | GET | Cached with IMemoryCache |
| `/products-hybrid-cache/{id}` | GET | Cached with HybridCache (stampede protected) |

## 🚦 Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQLite (for database, included in the project)
- K6 (for load testing, optional)

### Running the Application

1. Clone the repository
2. Navigate to the project directory
3. Run the application:
   ```bash
   cd StampedeProtection.Api/StampedeProtection.Api
   dotnet run
   ```
4. The API will be available at `https://localhost:7207`

## 🧪 Load Testing with K6

This project includes K6 scripts to demonstrate the cache stampede problem:

1. Install K6: https://k6.io/docs/get-started/installation/

2. Run the memory cache test (vulnerable to stampede):
   ```bash
   k6 run K6/script-memory-cache.js
   ```

3. Run the hybrid cache test (stampede protected):
   ```bash
   k6 run K6/script-hybrid-cache.js
   ```

## 📊 Understanding the Results

### Memory Cache (Vulnerable to Stampede)
- Multiple concurrent requests will all miss the cache
- Each request will trigger a database query
- Can lead to database overload
- Visible in logs as multiple "Getting Product {id} From Repository" messages

### Hybrid Cache (Stampede Protected)
- Only one request will execute the database query
- Other concurrent requests will wait for the result
- Prevents database overload
- Only one "Getting Product {id} From Repository" message per cache miss

## 🔍 Key Code Snippets

### Memory Cache (Vulnerable)
```csharp
if (_memoryCache.TryGetValue<Product>(id, out var cachedProduct))
{
    return cachedProduct;
}
// Multiple threads can reach here simultaneously
var product = await _productRepository.GetByIdAsync(id);
_memoryCache.Set(id, product, TimeSpan.FromMinutes(5));
```

### Hybrid Cache (Protected)
```csharp
// Only one thread will execute the value factory
return await _hybridCache.GetOrCreateAsync<Product>(id.ToString(), async (cancel) =>
{
    _logger.LogInformation("Getting Product {Id} From Repository", id);
    return await _productRepository.GetByIdAsync(id);
});
```

## 📝 Observations

1. **Under Low Load**:
   - All approaches perform similarly
   - Cache provides minor performance benefits

2. **Under High Load**:
   - No cache: Consistent but slow performance
   - Basic cache: Database overload during cache misses
   - Hybrid cache: Maintains performance during cache misses

3. **During Cache Expiration**:
   - Basic cache: Thundering herd problem
   - Hybrid cache: Only one request updates the cache

