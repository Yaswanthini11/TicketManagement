using Microsoft.Data.SqlClient;
using System.Data;
using TicketManagement.Helpers;
using TicketManagement.Models;

namespace TicketManagement.Repositories;

public class AuditLogRepository
{
    public async Task<List<AuditLog>> GetRecentAsync(int count = 10)
    {
        var list = new List<AuditLog>();
        const string sql = "SELECT TOP (@Count) AuditId, EntityName, EntityId, Action, Details, CreatedAt FROM AuditLogs ORDER BY CreatedAt DESC";
        await using var con = DatabaseHelper.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@Count", SqlDbType.Int).Value = count;
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
        {
            list.Add(new AuditLog
            {
                AuditId = r.GetInt64(0),
                EntityName = r.GetString(1),
                EntityId = r.GetInt32(2),
                Action = r.GetString(3),
                Details = r.IsDBNull(4) ? null : r.GetString(4),
                CreatedAt = r.GetDateTime(5)
            });
        }
        return list;
    }
}