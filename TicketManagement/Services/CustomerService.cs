using TicketManagement.Models;
using TicketManagement.Repositories;

namespace TicketManagement.Services;

public class CustomerService
{
    private readonly CustomerRepository _repo;
    public CustomerService(CustomerRepository repo) => _repo = repo;

    public Task<List<Customer>> GetAllAsync() => _repo.GetAllAsync();
    public Task<int> RegisterAsync(Customer c)
    {
        if (string.IsNullOrWhiteSpace(c.Name) || string.IsNullOrWhiteSpace(c.Email)) throw new ArgumentException("Name and Email are required.");
        return _repo.AddAsync(c);
    }
}