using Microsoft.Data.SqlClient;
using System.Data;
using TicketManagement.Helpers;
using TicketManagement.Models;

namespace TicketManagement.Repositories;

public class TicketUpdateRepository
{
    public async Task<int> AddAsync(TicketUpdate u)
    {
        const string sql = @"INSERT INTO TicketUpdates(TicketId, UpdatedBy, Message, OldStatus, NewStatus)
                             VALUES(@TicketId, @By, @Msg, @OldStatus, @NewStatus);
                             SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var con = DatabaseHelper.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@TicketId", SqlDbType.Int).Value = u.TicketId;
        cmd.Parameters.Add("@By", SqlDbType.NVarChar, 100).Value = u.UpdatedBy;
        cmd.Parameters.Add("@Msg", SqlDbType.NVarChar, -1).Value = u.Message;
        cmd.Parameters.Add("@OldStatus", SqlDbType.NVarChar, 20).Value = (object?)u.OldStatus ?? DBNull.Value;
        cmd.Parameters.Add("@NewStatus", SqlDbType.NVarChar, 20).Value = (object?)u.NewStatus ?? DBNull.Value;
        await con.OpenAsync();
        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    public async Task<List<TicketUpdate>> GetByTicketAsync(int ticketId)
    {
        var list = new List<TicketUpdate>();
        const string sql = "SELECT UpdateId, TicketId, UpdatedBy, Message, OldStatus, NewStatus, CreatedAt FROM TicketUpdates WHERE TicketId=@TicketId ORDER BY CreatedAt ASC";
        await using var con = DatabaseHelper.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@TicketId", SqlDbType.Int).Value = ticketId;
        await con.OpenAsync();
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
        {
            list.Add(new TicketUpdate
            {
                UpdateId = r.GetInt32(0),
                TicketId = r.GetInt32(1),
                UpdatedBy = r.GetString(2),
                Message = r.GetString(3),
                OldStatus = r.IsDBNull(4) ? null : r.GetString(4),
                NewStatus = r.IsDBNull(5) ? null : r.GetString(5),
                CreatedAt = r.GetDateTime(6)
            });
        }
        return list;
    }
}