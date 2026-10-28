namespace TicketManagement.Models;

public class TicketUpdate
{
    public int UpdateId { get; set; }
    public int TicketId { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? OldStatus { get; set; }
    public string? NewStatus { get; set; }
    public DateTime CreatedAt { get; set; }
}