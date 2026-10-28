using Microsoft.Data.SqlClient;
using System.Data;
using TicketManagement.Helpers;
using TicketManagement.Models;

namespace TicketManagement.Repositories;

public class TicketRepository
{
    public async Task<int> CreateTransactionalAsync(Ticket t, string initialNote)
    {
        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();
        await using var tx = await con.BeginTransactionAsync();

        try
        {
            const string sqlTicket = @"INSERT INTO Tickets (CustomerId, CategoryId, Subject, Description, Priority, Status)
                                       VALUES (@CustId, @CatId, @Sub, @Desc, @Prio, N'Open');
                                       SELECT CAST(SCOPE_IDENTITY() AS INT);";
            int ticketId;
            await using (var cmd = new SqlCommand(sqlTicket, con, (SqlTransaction)tx))
            {
                cmd.Parameters.Add("@CustId", SqlDbType.Int).Value = t.CustomerId;
                cmd.Parameters.Add("@CatId", SqlDbType.Int).Value = t.CategoryId;
                cmd.Parameters.Add("@Sub", SqlDbType.NVarChar, 200).Value = t.Subject;
                cmd.Parameters.Add("@Desc", SqlDbType.NVarChar, -1).Value = t.Description;
                cmd.Parameters.Add("@Prio", SqlDbType.NVarChar, 20).Value = t.Priority;
                ticketId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }

            const string sqlUpdate = @"INSERT INTO TicketUpdates (TicketId, UpdatedBy, Message, OldStatus, NewStatus)
                                       VALUES (@TicketId, N'Customer', @Msg, NULL, N'Open');";
            await using (var cmd = new SqlCommand(sqlUpdate, con, (SqlTransaction)tx))
            {
                cmd.Parameters.Add("@TicketId", SqlDbType.Int).Value = ticketId;
                cmd.Parameters.Add("@Msg", SqlDbType.NVarChar, -1).Value = initialNote;
                await cmd.ExecuteNonQueryAsync();
            }

            const string sqlAudit = @"INSERT INTO AuditLogs (EntityName, EntityId, Action, Details)
                                      VALUES (N'Ticket', @TicketId, N'INSERT', @Details);";
            await using (var cmd = new SqlCommand(sqlAudit, con, (SqlTransaction)tx))
            {
                cmd.Parameters.Add("@TicketId", SqlDbType.Int).Value = ticketId;
                cmd.Parameters.Add("@Details", SqlDbType.NVarChar, 500).Value = $"Created ticket: {t.Subject}";
                await cmd.ExecuteNonQueryAsync();
            }

            await tx.CommitAsync();
            return ticketId;
        }
        catch
        {
            await tx.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Ticket>> GetAllAsync()
    {
        var list = new List<Ticket>();
        const string sql = "SELECT TicketId, CustomerId, CategoryId, AgentId, Subject, Description, Priority, Status, CreatedAt, UpdatedAt FROM Tickets ORDER BY CreatedAt DESC";
        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();
        await using var cmd = new SqlCommand(sql, con);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
        {
            list.Add(new Ticket
            {
                TicketId = r.GetInt32(0),
                CustomerId = r.GetInt32(1),
                CategoryId = r.GetInt32(2),
                AgentId = r.IsDBNull(3) ? null : r.GetInt32(3),
                Subject = r.GetString(4),
                Description = r.GetString(5),
                Priority = r.GetString(6),
                Status = r.GetString(7),
                CreatedAt = r.GetDateTime(8),
                UpdatedAt = r.IsDBNull(9) ? null : r.GetDateTime(9)
            });
        }
        return list;
    }

    public async Task<bool> AssignAgentAsync(int ticketId, int agentId)
    {
        const string sql = "UPDATE Tickets SET AgentId=@AgentId, UpdatedAt=SYSDATETIME() WHERE TicketId=@TicketId";
        await using var con = DatabaseHelper.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@AgentId", SqlDbType.Int).Value = agentId;
        cmd.Parameters.Add("@TicketId", SqlDbType.Int).Value = ticketId;
        await con.OpenAsync();
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> UpdateStatusAsync(int ticketId, string status)
    {
        const string sql = "UPDATE Tickets SET Status=@Status, UpdatedAt=SYSDATETIME() WHERE TicketId=@TicketId";
        await using var con = DatabaseHelper.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 20).Value = status;
        cmd.Parameters.Add("@TicketId", SqlDbType.Int).Value = ticketId;
        await con.OpenAsync();
        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}