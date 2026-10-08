using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(Guid id);
}