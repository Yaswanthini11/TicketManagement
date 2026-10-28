namespace TicketManagement.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public DateTime RegisteredAt { get; set; }
    public bool IsActive { get; set; }
}