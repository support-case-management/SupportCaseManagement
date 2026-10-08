using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Infrastructure.Repositories;
/// <summary>
/// Tillfällig implementation av ICustomerRepository för att kunna köra och testa 
/// "Skapa ärende"-formuläret medan kollegan färdigställer JsonFileCustomerRepository.
/// </summary>
public class CustomerRepositoryTest : ICustomerRepository
{
    private readonly List<Customer> _dummyCustomers = new()
    {
        new Customer(Guid.NewGuid(), "Kund nr 1", "info@kund1.se", true),
        new Customer(Guid.NewGuid(), "Kund nr 2", "info@kund1.se", true),
        new Customer(Guid.NewGuid(), "Kund nr 1", "info@kund1.se", true),

    };

    public Task<List<Customer>> GetAllAsync()
    {
        return Task.FromResult(_dummyCustomers);
    }

    public Task<Customer?> GetByIdAsync(Guid id)
    {
        var customer = _dummyCustomers.FirstOrDefault(c => c.CustomerId == id);
        return Task.FromResult(customer);
    }
}