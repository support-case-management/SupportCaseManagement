namespace SupportCaseManagement.Application.Dtos.Requests;

public record CreateCustomerRequest
(
    string CustomerName,
    string EmailAddress,
    bool IsCompany
);
