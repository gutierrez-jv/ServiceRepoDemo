using ServiceRepoDemo.Models;

namespace ServiceRepoDemo.Services;

// Service = business logic. Controllers talk to this, never to the repository directly.
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<ServiceResult> CreateAsync(Product product);
    Task<ServiceResult> UpdateAsync(Product product);
}

public record ServiceResult(bool Success, string? Error = null)
{
    public static ServiceResult Ok() => new(true);
    public static ServiceResult Fail(string error) => new(false, error);
}
