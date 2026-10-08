using SupportCaseManagement.Application.Dtos.Requests;
using SupportCaseManagement.Application.Dtos.Responses;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Application.Features.Customers;

internal class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<CreateCustomerResponse> CreateCustomerAsync(CreateCustomerRequest request)
    {
        CreateCustomerResponse response;

        try
        {
            Guid customerId = Guid.NewGuid();

            Customer customer = new Customer(customerId, request.CustomerName, request.EmailAddress, request.IsCompany);

            List<Customer> customers = await _customerRepository.GetAllAsync();
            customers.Add(customer);

            await _customerRepository.SaveAllAsync(customers);

            response = new CreateCustomerResponse(true, customer, null);
        }
        catch (Exception ex)
        {
            response = new CreateCustomerResponse(false, null, ex.Message);
        }

        return response;
    }

    public async Task<GetAllCustomersResponse> GetAllCustomersAsync()
    {
        GetAllCustomersResponse response;

        try
        {
            List<Customer> customers = await _customerRepository.GetAllAsync();
            customers = [.. customers.OrderBy(customer => customer.CustomerName)];

            response = new GetAllCustomersResponse(true, customers, null);
        }
        catch (Exception ex)
        {
            response = new GetAllCustomersResponse(false, [], ex.Message);
        }

        return response;
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
