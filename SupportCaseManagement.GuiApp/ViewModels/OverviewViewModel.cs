using CommunityToolkit.Mvvm.ComponentModel;
using SupportCaseManagement.Application.Features.SupportTicketList;
using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Presentation.Navigation;
using System.Linq;
using System.Threading.Tasks;
namespace SupportCaseManagement.Presentation.ViewModels;

public partial class OverviewViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;

    private readonly ISupportTicketListService _ticketListService;

    [ObservableProperty]
    public partial int NewSupportTicketsCount { get; set; }

    [ObservableProperty]
    public partial int InProgressSupportTicketsCount { get; set; }

    [ObservableProperty]
    public partial int ResolvedSupportTicketsCount { get; set; }

    public OverviewViewModel(INavigationService navigationService, ISupportTicketListService ticketListService)
    {
        _navigationService = navigationService;
        _ticketListService = ticketListService;

    }

    public async Task LoadOverviewDataAsync()
    {
        var tickets = (await _ticketListService.GetAllTicketsSortedAsync()).ToList();

        NewSupportTicketsCount = tickets.Count(t => t.Status == TicketStatus.New);
        InProgressSupportTicketsCount = tickets.Count(t => t.Status == TicketStatus.InProgress);
        ResolvedSupportTicketsCount = tickets.Count(t => t.Status == TicketStatus.Closed);
    }


}

