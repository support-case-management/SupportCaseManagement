using SupportCaseManagement.Application.Dtos.Requests;
using SupportCaseManagement.Application.Dtos.Responses;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Application.Features.Customers;

public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;

    public async Task<CreateCustomerResponse> CreateCustomerAsync(CreateCustomerRequest request)
    {
        CreateCustomerResponse response;

        try
        {
            Guid customerId = Guid.NewGuid();

            Customer customer = new(customerId, request.CustomerName, request.EmailAddress, request.IsCompany);

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

    public async Task<GetCustomerByIdResponse> GetCustomerByIdAsync(Guid customerId)
    {
        GetCustomerByIdResponse response;

        try
        {
            Customer? customer = await _customerRepository.GetByIdAsync(customerId);

            if (customer is not null)
            {
                response = new GetCustomerByIdResponse(true, customer, null);
            }
            else
            {
                response = new GetCustomerByIdResponse(false, null, "Något gick fel, försök igen");
            }
        }
        catch (Exception ex)
        {
            response = new GetCustomerByIdResponse(false, null, ex.Message);
        }

        return response;
    }

    public async Task<UpdateCustomerResponse> UpdateCustomerAsync(UpdateCustomerRequest request)
    {
        UpdateCustomerResponse response;

        try
        {
            GetCustomerByIdResponse getCustomerResponse = await GetCustomerByIdAsync(request.CustomerId);
            Customer? customer = getCustomerResponse.Customer;

            if (getCustomerResponse.Success && customer is not null)
            {
                customer = UpdateCustomerName(request.CustomerName, customer);
                customer = UpdateCustomerEmail(request.EmailAddress, customer);
                customer = UpdateCustomerIsCompany(request.IsCompany, customer);

                await _customerRepository.UpdateAsync(customer);

                response = new UpdateCustomerResponse(true, customer, null);
            }
            else
            {
                response = new UpdateCustomerResponse(false, null, "Kunden kunde inte hittas");
            }
        }
        catch (Exception ex)
        {
            response = new UpdateCustomerResponse(false, null, ex.Message);
        }

        return response;
    }

    private Customer UpdateCustomerName(string customerName, Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customerName))
        {
            throw new ArgumentException("Kundens namn kan inte tas bort");
        }

        if (!customerName.Equals(customer.CustomerName))
        {
            customer.Rename(customerName);
        }

        return customer;
    }

    private Customer UpdateCustomerEmail(string emailAddress, Customer customer)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            throw new ArgumentException("Kundens e-postadress kan inte tas bort");
        }

        if (!emailAddress.Equals(customer.EmailAddress))
        {
            customer.ChangeEmailAddress(emailAddress);
        }

        return customer;
    }

    private Customer UpdateCustomerIsCompany(bool isCompany, Customer customer)
    {
        if (isCompany != customer.IsCompany)
        {
            customer.ChangeIsCompany(isCompany);
        }

        return customer;
    }
}
