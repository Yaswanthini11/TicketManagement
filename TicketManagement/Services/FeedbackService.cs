using TicketManagement.Models;
using TicketManagement.Repositories;

namespace TicketManagement.Services;

public class FeedbackService
{
    private readonly FeedbackRepository _repo;
    public FeedbackService(FeedbackRepository repo) => _repo = repo;

    public Task<List<Feedback>> GetAllAsync() => _repo.GetAllAsync();
    public Task<int> AddAsync(Feedback f)
    {
        if (f.TicketId <= 0 || f.Rating < 1 || f.Rating > 5) throw new ArgumentException("Valid Ticket ID and Rating (1-5) are required.");
        return _repo.AddAsync(f);
    }
}