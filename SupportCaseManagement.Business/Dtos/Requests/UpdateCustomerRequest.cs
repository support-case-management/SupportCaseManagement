namespace SupportCaseManagement.Application.Dtos.Requests;

public record UpdateCustomerRequest
(
    Guid CustomerId,
    string CustomerName,
    string EmailAddress,
    bool IsCompany
);
