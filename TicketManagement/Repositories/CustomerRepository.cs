using Microsoft.Data.SqlClient;
using System.Data;
using TicketManagement.Helpers;
using TicketManagement.Models;

namespace TicketManagement.Repositories;

public class CustomerRepository
{
    public async Task<int> AddAsync(Customer c)
    {
        const string sql = "INSERT INTO Customers(Name, Email, Phone) VALUES(@Name, @Email, @Phone); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var con = DatabaseHelper.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = c.Name;
        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = c.Email;
        cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 30).Value = (object?)c.Phone ?? DBNull.Value;
        await con.OpenAsync();
        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        var list = new List<Customer>();
        const string sql = "SELECT CustomerId, Name, Email, Phone, RegisteredAt, IsActive FROM Customers ORDER BY Name";
        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();
        await using var cmd = new SqlCommand(sql, con);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            list.Add(new Customer { CustomerId = r.GetInt32(0), Name = r.GetString(1), Email = r.GetString(2), Phone = r.IsDBNull(3) ? null : r.GetString(3), RegisteredAt = r.GetDateTime(4), IsActive = r.GetBoolean(5) });
        return list;
    }
}