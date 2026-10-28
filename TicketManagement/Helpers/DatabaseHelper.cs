using Microsoft.Data.SqlClient;

namespace TicketManagement.Helpers;

public static class DatabaseHelper
{
    private const string ConnectionString =
        @"Server=.\SQLEXPRESS;Database=TicketDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public static SqlConnection CreateConnection() => new(ConnectionString);
}