using TicketManagement.Models;
using TicketManagement.Repositories;

namespace TicketManagement.Services;

public class CategoryService
{
    private readonly CategoryRepository _repo;
    public CategoryService(CategoryRepository repo) => _repo = repo;

    public Task<List<Category>> GetAllAsync() => _repo.GetAllAsync();
    public Task<int> AddAsync(Category c)
    {
        if (string.IsNullOrWhiteSpace(c.CategoryName)) throw new ArgumentException("Category name is required.");
        return _repo.AddAsync(c);
    }
}