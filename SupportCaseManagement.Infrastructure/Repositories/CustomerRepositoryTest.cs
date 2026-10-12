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

    public Task SaveAllAsync(IEnumerable<Customer> customers)
    {
        var updatedList = customers.ToList();

        _dummyCustomers.Clear();
        _dummyCustomers.AddRange(updatedList);

        return Task.CompletedTask;
    }

    public Task<Customer> UpdateAsync(Customer updatedCustomer)
    {
        int index = _dummyCustomers.FindIndex(
            customer => customer.CustomerId == updatedCustomer.CustomerId);

        if (index == -1)
        {
            throw new InvalidOperationException("Kunden kunde inte hittas");
        }

        _dummyCustomers[index] = updatedCustomer;

        return Task.FromResult(updatedCustomer);
    }

}