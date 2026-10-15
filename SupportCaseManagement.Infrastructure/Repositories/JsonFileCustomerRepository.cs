using SupportCaseManagement.Application.Features.Settings;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;
using System.Text.Json;

namespace SupportCaseManagement.Infrastructure.Repositories;

internal class JsonFileCustomerRepository(ISettingsService settingsService) : ICustomerRepository
{

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public async Task SaveAllAsync(IEnumerable<Customer> customers)
    {
        var filePath = settingsService.CustomerFilePath;

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        string json = JsonSerializer.Serialize(customers, _options);
        var tempPath = filePath + ".tmp";

        await File.WriteAllTextAsync(tempPath, json);
        File.Move(tempPath, filePath, overwrite: true);
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        var filePath = settingsService.CustomerFilePath;

        if (!File.Exists(filePath))
        {
            return [];
        }

        string json = await File.ReadAllTextAsync(filePath);

        List<Customer> customers = JsonSerializer.Deserialize<List<Customer>>(json, _options)
            ?? throw new JsonException("Kundlistans fil måste innehålla en json baserad lista");

        return customers;
    }

    public async Task<Customer?> GetByIdAsync(Guid id)
    {
        List<Customer> customers = await GetAllAsync();

        Customer? customer = customers.FirstOrDefault(c => c.CustomerId == id);

        if (customer is null)
        {
            throw new ArgumentNullException($"Ingen kund med id '{id}' kunde hittas i den sparade filen");
        }

        return customer;
    }

    public async Task<Customer> UpdateAsync(Customer updatedCustomer)
    {
        if (updatedCustomer is null)
        {
            throw new ArgumentNullException("Kunden kan inte ändras till null");
        }

        List<Customer> customers = await GetAllAsync();
        int index;

        for (index = 0; index < customers.Count; index++)
        {
            if (customers[index].CustomerId == updatedCustomer.CustomerId)
            {
                customers[index] = updatedCustomer;
                break;
            }
        }

        if (index == customers.Count)
        {
            throw new InvalidOperationException("Kunden kunde inte hittas i den sparade filen");
        }

        await SaveAllAsync(customers);

        return customers[index];
    }
}
