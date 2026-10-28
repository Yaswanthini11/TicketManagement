using TicketManagement.Models;
using TicketManagement.Repositories;

namespace TicketManagement.Services;

public class TicketUpdateService
{
    private readonly TicketUpdateRepository _repo;
    public TicketUpdateService(TicketUpdateRepository repo) => _repo = repo;

    public Task<List<TicketUpdate>> GetByTicketAsync(int ticketId) => _repo.GetByTicketAsync(ticketId);
    public Task<int> AddUpdateAsync(TicketUpdate u)
    {
        if (u.TicketId <= 0 || string.IsNullOrWhiteSpace(u.Message)) throw new ArgumentException("Ticket ID and Message are required.");
        return _repo.AddAsync(u);
    }
}