using SupportCaseManagement.Application.Dtos.Requests;
using SupportCaseManagement.Application.Dtos.Responses;

namespace SupportCaseManagement.Application.Features.Customers;

internal class CustomerService : ICustomerService
{
    public Task<CreateCustomerResponse> CreateCustomerAsync(CreateCustomerRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<GetAllCustomersResponse> GetAllCustomersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GetCustomerByIdResponse> GetCustomerByIdAsync(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateCustomerResponse> UpdateCustomerAsync(UpdateCustomerRequest request)
    {
        throw new NotImplementedException();
    }
}
