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

    public async Task<List<Category>> GetAllAsync()
    {
        var list = new List<Category>();
        const string sql = "SELECT CategoryId, CategoryName, Description, IsActive FROM Categories ORDER BY CategoryName";
        await using var con = DatabaseHelper.CreateConnection();
        await con.OpenAsync();
        await using var cmd = new SqlCommand(sql, con);
        await using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            list.Add(new Category { CategoryId = r.GetInt32(0), CategoryName = r.GetString(1), Description = r.IsDBNull(2) ? null : r.GetString(2), IsActive = r.GetBoolean(3) });
        return list;
    }
}