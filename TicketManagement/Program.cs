using TicketManagement.Models;
using TicketManagement.Repositories;
using TicketManagement.Services;

var catService = new CategoryService(new CategoryRepository());
var custService = new CustomerService(new CustomerRepository());
var agentService = new SupportAgentService(new SupportAgentRepository());
var ticketService = new TicketService(new TicketRepository());
var updateService = new TicketUpdateService(new TicketUpdateRepository());
var feedbackService = new FeedbackService(new FeedbackRepository());
var auditService = new AuditLogService(new AuditLogRepository());

while (true)
{
    Console.WriteLine("\n==== CUSTOMER COMPLAINT & TICKET MANAGEMENT ====");
    Console.WriteLine("1. Categories (Add / List)");
    Console.WriteLine("2. Customers (Register / List)");
    Console.WriteLine("3. Support Agents (Add / List)");
    Console.WriteLine("4. Tickets (Create / List / Assign / Update Status)");
    Console.WriteLine("5. Ticket History (View Timeline)");
    Console.WriteLine("6. Feedbacks (Submit / View)");
    Console.WriteLine("7. Recent Audit Logs");
    Console.WriteLine("0. Exit");
    Console.Write("Choice: ");

    string? choice = Console.ReadLine();
    try
    {
        switch (choice)
        {
            case "1":
                Console.WriteLine("1. Add Category | 2. List Categories");
                if (Console.ReadLine() == "1")
                {
                    var c = new Category();
                    Console.Write("Name: "); c.CategoryName = Console.ReadLine() ?? "";
                    Console.Write("Description: "); c.Description = Console.ReadLine();
                    Console.WriteLine($"Category Created ID: {await catService.AddAsync(c)}");
                }
                else
                {
                    foreach (var cat in await catService.GetAllAsync())
                        Console.WriteLine($"{cat.CategoryId} | {cat.CategoryName} | {cat.Description}");
                }
                break;

            case "2":
                Console.WriteLine("1. Register Customer | 2. List Customers");
                if (Console.ReadLine() == "1")
                {
                    var cust = new Customer();
                    Console.Write("Name: "); cust.Name = Console.ReadLine() ?? "";
                    Console.Write("Email: "); cust.Email = Console.ReadLine() ?? "";
                    Console.Write("Phone: "); cust.Phone = Console.ReadLine();
                    Console.WriteLine($"Customer Registered ID: {await custService.RegisterAsync(cust)}");
                }
                else
                {
                    foreach (var cust in await custService.GetAllAsync())
                        Console.WriteLine($"{cust.CustomerId} | {cust.Name} | {cust.Email}");
                }
                break;

            case "3":
                Console.WriteLine("1. Add Agent | 2. List Agents");
                if (Console.ReadLine() == "1")
                {
                    var a = new SupportAgent();
                    Console.Write("Name: "); a.Name = Console.ReadLine() ?? "";
                    Console.Write("Email: "); a.Email = Console.ReadLine() ?? "";
                    Console.Write("Department: "); a.Department = Console.ReadLine() ?? "";
                    Console.WriteLine($"Agent Added ID: {await agentService.AddAsync(a)}");
                }
                else
                {
                    foreach (var ag in await agentService.GetAllAsync())
                        Console.WriteLine($"{ag.AgentId} | {ag.Name} | {ag.Department}");
                }
                break;

            case "4":
                Console.WriteLine("1. Create Ticket | 2. List Tickets | 3. Assign Agent | 4. Update Status");
                string? sub = Console.ReadLine();
                if (sub == "1")
                {
                    var t = new Ticket();
                    Console.Write("Customer ID: "); t.CustomerId = int.Parse(Console.ReadLine()!);
                    Console.Write("Category ID: "); t.CategoryId = int.Parse(Console.ReadLine()!);
                    Console.Write("Subject: "); t.Subject = Console.ReadLine() ?? "";
                    Console.Write("Description: "); t.Description = Console.ReadLine() ?? "";
                    Console.Write("Priority (Low/Medium/High/Urgent): "); t.Priority = Console.ReadLine() ?? "Medium";
                    Console.Write("Initial Note: "); string note = Console.ReadLine() ?? "Created";
                    Console.WriteLine($"Ticket Created ID: {await ticketService.CreateTicketAsync(t, note)}");
                }
                else if (sub == "2")
                {
                    foreach (var tk in await ticketService.GetAllAsync())
                        Console.WriteLine($"[#{tk.TicketId}] {tk.Subject} | Priority: {tk.Priority} | Status: {tk.Status} | AgentId: {tk.AgentId}");
                }
                else if (sub == "3")
                {
                    Console.Write("Ticket ID: "); int tid = int.Parse(Console.ReadLine()!);
                    Console.Write("Agent ID: "); int aid = int.Parse(Console.ReadLine()!);
                    Console.WriteLine(await ticketService.AssignAgentAsync(tid, aid) ? "Agent Assigned." : "Failed.");
                }
                else if (sub == "4")
                {
                    Console.Write("Ticket ID: "); int tid = int.Parse(Console.ReadLine()!);
                    Console.Write("Status (Open/In Progress/Resolved/Closed): "); string st = Console.ReadLine() ?? "";
                    Console.WriteLine(await ticketService.UpdateStatusAsync(tid, st) ? "Status Updated." : "Failed.");
                }
                break;

            case "5":
                Console.Write("Ticket ID: "); int ticketId = int.Parse(Console.ReadLine()!);
                foreach (var up in await updateService.GetByTicketAsync(ticketId))
                    Console.WriteLine($"{up.CreatedAt:G} | {up.UpdatedBy}: {up.Message} [{up.OldStatus} -> {up.NewStatus}]");
                break;

            case "6":
                Console.WriteLine("1. Submit Feedback | 2. View All Feedback");
                if (Console.ReadLine() == "1")
                {
                    var fb = new Feedback();
                    Console.Write("Ticket ID: "); fb.TicketId = int.Parse(Console.ReadLine()!);
                    Console.Write("Rating (1-5): "); fb.Rating = int.Parse(Console.ReadLine()!);
                    Console.Write("Comments: "); fb.Comments = Console.ReadLine();
                    Console.WriteLine($"Feedback Submitted ID: {await feedbackService.AddAsync(fb)}");
                }
                else
                {
                    foreach (var fb in await feedbackService.GetAllAsync())
                        Console.WriteLine($"Ticket #{fb.TicketId} | Rating: {fb.Rating}/5 | {fb.Comments}");
                }
                break;

            case "7":
                foreach (var log in await auditService.GetRecentAsync())
                    Console.WriteLine($"{log.CreatedAt:G} | {log.EntityName} #{log.EntityId} | {log.Action} | {log.Details}");
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Invalid selection.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}