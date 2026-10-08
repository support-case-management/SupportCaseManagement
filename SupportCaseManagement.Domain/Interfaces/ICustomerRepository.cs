using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Domain.Interfaces;

public interface ICustomerRepository
{
    Task SaveAllAsync(IEnumerable<Customer> customers);
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);
    Task<SupportTicket> UpdateAsync(Customer updatedCustomer);
}
