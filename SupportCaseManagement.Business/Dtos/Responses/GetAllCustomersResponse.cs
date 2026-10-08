using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Application.Dtos.Responses;

public record GetAllCustomersResponse
(
    bool Success,
    List<Customer> Customers,
    string? ErrorMessage
);
