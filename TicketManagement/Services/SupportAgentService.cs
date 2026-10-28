using TicketManagement.Models;
using TicketManagement.Repositories;

namespace TicketManagement.Services;

public class SupportAgentService
{
    private readonly SupportAgentRepository _repo;
    public SupportAgentService(SupportAgentRepository repo) => _repo = repo;

    public Task<List<SupportAgent>> GetAllAsync() => _repo.GetAllAsync();
    public Task<int> AddAsync(SupportAgent a)
    {
        if (string.IsNullOrWhiteSpace(a.Name) || string.IsNullOrWhiteSpace(a.Email)) throw new ArgumentException("Name and Email are required.");
        return _repo.AddAsync(a);
    }
}