
using EcommerceManagement.Helpers;
using EcommerceManagement.Models;
using Microsoft.Data.SqlClient;

namespace EcommerceManagement.Repositories;

public class CategoryRepository
{
    public async Task<int> AddAsync(Category category)
    {
        const string sql = """
            INSERT INTO Categories (CategoryName, Description)
            VALUES (@CategoryName, @Description);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using var connection =
            DatabaseHelper.CreateConnection();

        await connection.OpenAsync();

        await using var command =
            new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@CategoryName", category.CategoryName);

        command.Parameters.AddWithValue(
            "@Description",
            (object?)category.Description ?? DBNull.Value);

        var result = await command.ExecuteScalarAsync();

        return Convert.ToInt32(result);
    }

    public async Task<List<Category>> GetAllAsync()
    {
        const string sql = """
            SELECT CategoryId, CategoryName, Description,
                   IsActive, CreatedAt
            FROM Categories
            ORDER BY CategoryId;
            """;

        var categories = new List<Category>();

        await using var connection =
            DatabaseHelper.CreateConnection();

        await connection.OpenAsync();

        await using var command =
            new SqlCommand(sql, connection);

        await using var reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            categories.Add(new Category
            {
                CategoryId = reader.GetInt32(0),
                CategoryName = reader.GetString(1),
                Description = reader.IsDBNull(2)
                    ? null
                    : reader.GetString(2),
                IsActive = reader.GetBoolean(3),
                CreatedAt = reader.GetDateTime(4)
            });
        }

        return categories;
    }

    public async Task<Category?> GetByIdAsync(int categoryId)
    {
        const string sql = """
            SELECT CategoryId, CategoryName, Description,
                   IsActive, CreatedAt
            FROM Categories
            WHERE CategoryId = @CategoryId;
            """;

        await using var connection =
            DatabaseHelper.CreateConnection();

        await connection.OpenAsync();

        await using var command =
            new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@CategoryId", categoryId);

        await using var reader =
            await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
            return null;

        return new Category
        {
            CategoryId = reader.GetInt32(0),
            CategoryName = reader.GetString(1),
            Description = reader.IsDBNull(2)
                ? null
                : reader.GetString(2),
            IsActive = reader.GetBoolean(3),
            CreatedAt = reader.GetDateTime(4)
        };
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        const string sql = """
            UPDATE Categories
            SET CategoryName = @CategoryName,
                Description = @Description
            WHERE CategoryId = @CategoryId
              AND IsActive = 1;
            """;

        await using var connection =
            DatabaseHelper.CreateConnection();

        await connection.OpenAsync();

        await using var command =
            new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@CategoryId", category.CategoryId);

        command.Parameters.AddWithValue(
            "@CategoryName", category.CategoryName);

        command.Parameters.AddWithValue(
            "@Description",
            (object?)category.Description ?? DBNull.Value);

        int rows = await command.ExecuteNonQueryAsync();

        return rows > 0;
    }

    public async Task<bool> DeactivateAsync(int categoryId)
    {
        const string sql = """
            UPDATE Categories
            SET IsActive = 0
            WHERE CategoryId = @CategoryId
              AND IsActive = 1;
            """;

        await using var connection =
            DatabaseHelper.CreateConnection();

        await connection.OpenAsync();

        await using var command =
            new SqlCommand(sql, connection);

        command.Parameters.AddWithValue(
            "@CategoryId", categoryId);

        int rows = await command.ExecuteNonQueryAsync();

        return rows > 0;
    }
}
