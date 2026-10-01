using Microsoft.Extensions.DependencyInjection;
using SupportCaseManagement.Application.CaseWorker;
using SupportCaseManagement.Application.Features.SupportTicketList;
namespace SupportCaseManagement.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<CaseWorkerSercice>();
        services.AddSingleton<ISupportTicketListService, SupportTicketListService>();
        return services;
    }
}
