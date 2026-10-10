using Microsoft.Extensions.DependencyInjection;
using SupportCaseManagement.Application.CaseWorker;
using SupportCaseManagement.Application.Features.Customers;
using SupportCaseManagement.Application.Features.SupportTicketList;
namespace SupportCaseManagement.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<CaseWorkerService>();
        services.AddSingleton<ISupportTicketListService, SupportTicketListService>();
        services.AddSingleton<ICustomerService, CustomerService>();
        return services;
    }
}
