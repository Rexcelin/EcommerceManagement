
using EcommerceManagement.Models;
using EcommerceManagement.Repositories;

namespace EcommerceManagement.Services;

public class CategoryService
{
    private readonly CategoryRepository _repository;

    public CategoryService()
    {
        _repository = new CategoryRepository();
    }

    public async Task<int> AddAsync(
        string name, string? description)
    {
        name = name.Trim();
        description = string.IsNullOrWhiteSpace(description)
            ? null
            : description.Trim();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Category name is required.");

        if (name.Length > 100)
            throw new ArgumentException(
                "Category name cannot exceed 100 characters.");

        if (description?.Length > 500)
            throw new ArgumentException(
                "Description cannot exceed 500 characters.");

        var category = new Category
        {
            CategoryName = name,
            Description = description
        };

        return await _repository.AddAsync(category);
    }

    public Task<List<Category>> GetAllAsync()
    {
        return _repository.GetAllAsync();
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(
                "Category ID must be greater than zero.");

        var category = await _repository.GetByIdAsync(id);

        return category ?? throw new ArgumentException(
            "Category not found.");
    }

    public async Task<bool> UpdateAsync(
        int id, string name, string? description)
    {
        if (id <= 0)
            throw new ArgumentException(
                "Category ID must be greater than zero.");

        name = name.Trim();
        description = string.IsNullOrWhiteSpace(description)
            ? null
            : description.Trim();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Category name is required.");

        if (name.Length > 100)
            throw new ArgumentException(
                "Category name cannot exceed 100 characters.");

        if (description?.Length > 500)
            throw new ArgumentException(
                "Description cannot exceed 500 characters.");

        return await _repository.UpdateAsync(new Category
        {
            CategoryId = id,
            CategoryName = name,
            Description = description
        });
    }

    public async Task<bool> DeactivateAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException(
                "Category ID must be greater than zero.");

        return await _repository.DeactivateAsync(id);
    }
}
