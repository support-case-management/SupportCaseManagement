using Microsoft.Extensions.DependencyInjection;
using SupportCaseManagement.Application.CaseWorker;

namespace SupportCaseManagement.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<CaseWorkerService>();
        return services;
    }
}
