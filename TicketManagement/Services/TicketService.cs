using TicketManagement.Models;
using TicketManagement.Repositories;

namespace TicketManagement.Services;

public class TicketService
{
    private readonly TicketRepository _repo;
    public TicketService(TicketRepository repo) => _repo = repo;

    public Task<List<Ticket>> GetAllAsync() => _repo.GetAllAsync();

    public Task<int> CreateTicketAsync(Ticket t, string initialNote)
    {
        if (t.CustomerId <= 0 || t.CategoryId <= 0 || string.IsNullOrWhiteSpace(t.Subject))
            throw new ArgumentException("Valid Customer ID, Category ID, and Subject are required.");
        return _repo.CreateTransactionalAsync(t, initialNote);
    }

    public Task<bool> AssignAgentAsync(int ticketId, int agentId)
    {
        if (ticketId <= 0 || agentId <= 0) throw new ArgumentException("Invalid ticket or agent ID.");
        return _repo.AssignAgentAsync(ticketId, agentId);
    }

    public Task<bool> UpdateStatusAsync(int ticketId, string status)
    {
        if (!new[] { "Open", "In Progress", "Resolved", "Closed" }.Contains(status))
            throw new ArgumentException("Invalid status value.");
        return _repo.UpdateStatusAsync(ticketId, status);
    }
}