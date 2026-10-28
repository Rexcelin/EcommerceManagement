
using EcommerceManagement.Models;
using EcommerceManagement.Services;
using Microsoft.Data.SqlClient;

var categoryService = new CategoryService();

while (true)
{
    Console.WriteLine("\n===== E-COMMERCE MANAGEMENT SYSTEM =====");
    Console.WriteLine("1. Category Management");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");

    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            await CategoryMenuAsync(categoryService);
            break;

        case "0":
            Console.WriteLine("Thank you!");
            return;

        default:
            Console.WriteLine("Invalid choice. Try again.");
            break;
    }
}

static async Task CategoryMenuAsync(
    CategoryService service)
{
    while (true)
    {
        Console.WriteLine("\n===== CATEGORY MANAGEMENT =====");
        Console.WriteLine("1. Add Category");
        Console.WriteLine("2. View All Categories");
        Console.WriteLine("3. View Category By ID");
        Console.WriteLine("4. Update Category");
        Console.WriteLine("5. Deactivate Category");
        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Enter your choice: ");

        string? choice = Console.ReadLine();

        try
        {
            switch (choice)
            {
                case "1":
                    {
                        Console.Write("Category name: ");
                        string name = Console.ReadLine() ?? "";

                        Console.Write("Description (optional): ");
                        string? description = Console.ReadLine();

                        int id = await service.AddAsync(
                            name, description);

                        Console.WriteLine(
                            $"Category created successfully. ID: {id}");
                        break;
                    }

                case "2":
                    {
                        List<Category> categories =
                            await service.GetAllAsync();

                        if (categories.Count == 0)
                        {
                            Console.WriteLine("No categories found.");
                            break;
                        }

                        foreach (Category category in categories)
                        {
                            DisplayCategory(category);
                        }

                        break;
                    }

                case "3":
                    {
                        int id = ReadId();

                        Category category =
                            await service.GetByIdAsync(id);

                        DisplayCategory(category);
                        break;
                    }

                case "4":
                    {
                        int id = ReadId();

                        Category existing =
                            await service.GetByIdAsync(id);

                        if (!existing.IsActive)
                        {
                            Console.WriteLine(
                                "Cannot update an inactive category.");
                            break;
                        }

                        Console.Write(
                            $"New name [{existing.CategoryName}]: ");
                        string? nameInput = Console.ReadLine();

                        Console.Write(
                            $"New description [{existing.Description}]: ");
                        string? descriptionInput = Console.ReadLine();

                        string name = string.IsNullOrWhiteSpace(nameInput)
                            ? existing.CategoryName
                            : nameInput;

                        string? description =
                            string.IsNullOrWhiteSpace(descriptionInput)
                                ? existing.Description
                                : descriptionInput;

                        bool updated = await service.UpdateAsync(
                            id, name, description);

                        Console.WriteLine(updated
                            ? "Category updated successfully."
                            : "Category was not updated.");

                        break;
                    }

                case "5":
                    {
                        int id = ReadId();

                        Category existing =
                            await service.GetByIdAsync(id);

                        if (!existing.IsActive)
                        {
                            Console.WriteLine(
                                "Category is already inactive.");
                            break;
                        }

                        Console.Write(
                            $"Deactivate '{existing.CategoryName}'? (y/n): ");

                        if (Console.ReadLine()?.Trim()
                            .Equals("y", StringComparison.OrdinalIgnoreCase)
                            == true)
                        {
                            bool deactivated =
                                await service.DeactivateAsync(id);

                            Console.WriteLine(deactivated
                                ? "Category deactivated successfully."
                                : "Category was not deactivated.");
                        }
                        else
                        {
                            Console.WriteLine("Operation cancelled.");
                        }

                        break;
                    }

                case "0":
                    return;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        catch (SqlException ex)
        {
            if (ex.Number is 2601 or 2627)
            {
                Console.WriteLine(
                    "A category with that name may already exist.");
            }
            else
            {
                Console.WriteLine(
                    $"Database error: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

static int ReadId()
{
    Console.Write("Enter category ID: ");

    if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
    {
        throw new ArgumentException(
            "Please enter a valid positive ID.");
    }

    return id;
}

static void DisplayCategory(Category category)
{
    Console.WriteLine("\n------------------------------");
    Console.WriteLine($"ID          : {category.CategoryId}");
    Console.WriteLine($"Name        : {category.CategoryName}");
    Console.WriteLine($"Description : {category.Description ?? "-"}");
    Console.WriteLine($"Status      : {(category.IsActive ? "Active" : "Inactive")}");
    Console.WriteLine($"Created At  : {category.CreatedAt}");
    Console.WriteLine("------------------------------");
}
