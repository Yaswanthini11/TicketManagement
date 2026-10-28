using Microsoft.Data.SqlClient;
using System.Data;
using TicketManagement.Helpers;
using TicketManagement.Models;

namespace TicketManagement.Repositories;

public class SupportAgentRepository
{
    public async Task<int> AddAsync(SupportAgent a)
    {
        const string sql = "INSERT INTO SupportAgents(Name, Email, Department) VALUES(@Name, @Email, @Dept); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var con = DatabaseHelper.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = a.Name;
        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = a.Email;
        cmd.Parameters.Add("@Dept", SqlDbType.NVarChar, 50).Value = a.Department;
        await con.OpenAsync();
        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    public async Task<List<SupportAgent>> GetAllAsync()
    {
        var list = new List<SupportAgent>();
        const string sql = "SELECT AgentId, Name, Email, Department, IsActive FROM SupportAgents ORDER BY Name";
        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();
        await using var cmd = new SqlCommand(sql, con);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            list.Add(new SupportAgent { AgentId = r.GetInt32(0), Name = r.GetString(1), Email = r.GetString(2), Department = r.GetString(3), IsActive = r.GetBoolean(4) });
        return list;
    }
}