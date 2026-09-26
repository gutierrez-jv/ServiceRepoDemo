using Microsoft.EntityFrameworkCore;
using ServiceRepoDemo.Data;
using ServiceRepoDemo.Models;
using ServiceRepoDemo.Repositories;

namespace ServiceRepoDemo.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Product>> GetAllAsync() => _repository.GetAllAsync();

    public Task<Product?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    public async Task<ServiceResult> CreateAsync(Product product)
    {
        product.Name = product.Name?.Trim();
        var products = await _repository.GetAllAsync();

        if (products.Any(p => p.Name == product.Name))
        {
            return ServiceResult.Fail($"A product named '{product.Name}' already exists.");
        }
        product.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(product);
        await _repository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> UpdateAsync(Product product)
    {
        var existing = await _repository.GetByIdAsync(product.Id);

        if (existing is null)
            return ServiceResult.Fail("Product not found.");
        product.Name = product.Name?.Trim();
        var products = await _repository.GetAllAsync();

        if (products.Any(p => p.Name == product.Name && p.Id != product.Id))
        {
            return ServiceResult.Fail($"A product named '{product.Name}' already exists.");
        }

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Stock = product.Stock;
        existing.CreatedAt = product.CreatedAt;

        _repository.Update(existing);
        await _repository.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> DeleteAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product is null)
            return ServiceResult.Fail("Product not found.");

        if (product.Stock > 0)
        {
            return ServiceResult.Fail("Cannot delete a product that still has stock.");
        }

        _repository.Delete(product);
        await _repository.SaveChangesAsync();

        return ServiceResult.Ok();
    }
}
