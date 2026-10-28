using TicketManagement.Models;
using TicketManagement.Repositories;

namespace TicketManagement.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _repo;
    public AuditLogService(AuditLogRepository repo) => _repo = repo;

    public Task<List<AuditLog>> GetRecentAsync(int count = 10) => _repo.GetRecentAsync(count);
}