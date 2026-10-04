using Microsoft.Extensions.DependencyInjection;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Infrastructure.Repositories;

namespace SupportCaseManagement.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ISupportTicketRepository, JsonFileSupportTicketRepository>();

        return services;
    }
}

