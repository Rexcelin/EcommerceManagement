
using EcommerceManagement.Helpers;

try
{
    await using var connection =
        DatabaseHelper.CreateConnection();

    await connection.OpenAsync();

    Console.WriteLine("Connected to SQL Server successfully!");
    Console.WriteLine($"Database: {connection.Database}");
}
catch (Exception ex)
{
    Console.WriteLine("Database connection failed.");
    Console.WriteLine(ex.Message);
}
