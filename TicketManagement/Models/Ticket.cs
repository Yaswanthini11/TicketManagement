namespace TicketManagement.Models;

public class Ticket
{
    public int TicketId { get; set; }
    public int CustomerId { get; set; }
    public int CategoryId { get; set; }
    public int? AgentId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}