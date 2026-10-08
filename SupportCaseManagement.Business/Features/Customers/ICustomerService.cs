using SupportCaseManagement.Application.Dtos.Requests;
using SupportCaseManagement.Application.Dtos.Responses;

namespace SupportCaseManagement.Application.Features.Customers;

public interface ICustomerService
{
    Task<CreateCustomerResponse> CreateCustomerAsync(CreateCustomerRequest request);
    Task<GetAllCustomersResponse> GetAllCustomersAsync();
    Task<GetCustomerByIdResponse> GetCustomerByIdAsync(Guid customerId);
    Task<UpdateCustomerResponse> UpdateCustomerAsync(UpdateCustomerRequest request);
}