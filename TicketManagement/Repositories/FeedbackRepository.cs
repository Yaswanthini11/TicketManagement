using Microsoft.Data.SqlClient;
using System.Data;
using TicketManagement.Helpers;
using TicketManagement.Models;

namespace TicketManagement.Repositories;

public class FeedbackRepository
{
    public async Task<int> AddAsync(Feedback f)
    {
        const string sql = "INSERT INTO Feedbacks(TicketId, Rating, Comments) VALUES(@TicketId, @Rating, @Comments); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var con = DatabaseHelper.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@TicketId", SqlDbType.Int).Value = f.TicketId;
        cmd.Parameters.Add("@Rating", SqlDbType.Int).Value = f.Rating;
        cmd.Parameters.Add("@Comments", SqlDbType.NVarChar, 500).Value = (object?)f.Comments ?? DBNull.Value;
        await con.OpenAsync();
        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    public async Task<List<Feedback>> GetAllAsync()
    {
        var list = new List<Feedback>();
        const string sql = "SELECT FeedbackId, TicketId, Rating, Comments, SubmittedAt FROM Feedbacks ORDER BY SubmittedAt DESC";
        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();
        await using var cmd = new SqlCommand(sql, con);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
        {
            list.Add(new Feedback
            {
                FeedbackId = r.GetInt32(0),
                TicketId = r.GetInt32(1),
                Rating = r.GetInt32(2),
                Comments = r.IsDBNull(3) ? null : r.GetString(3),
                SubmittedAt = r.GetDateTime(4)
            });
        }
        return list;
    }
}