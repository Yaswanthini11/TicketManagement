using Microsoft.Data.SqlClient;
using System.Data;
using TicketManagement.Helpers;
using TicketManagement.Models;

namespace TicketManagement.Repositories;

public class CategoryRepository
{
    public async Task<int> AddAsync(Category c)
    {
        const string sql = "INSERT INTO Categories(CategoryName, Description) VALUES(@Name, @Desc); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var con = DatabaseHelper.CreateConnection();
        await using var cmd = new SqlCommand(sql, con);
        cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = c.CategoryName;
        cmd.Parameters.Add("@Desc", SqlDbType.NVarChar, 255).Value = (object?)c.Description ?? DBNull.Value;
        await con.OpenAsync();
        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }
}