using Microsoft.Extensions.DependencyInjection;
using SupportCaseManagement.Application.Features.SupportTicketList;

namespace SupportCaseManagement.Presentation;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ISupportTicketListService, SupportTicketListService>();

        return services;
    }
}
