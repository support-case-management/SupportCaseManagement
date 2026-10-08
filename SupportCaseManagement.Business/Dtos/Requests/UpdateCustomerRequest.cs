namespace SupportCaseManagement.Application.Dtos.Requests;

public record UpdateCustomerRequest
(
    string CustomerName,
    string EmailAddress,
    bool IsCompany
);
