using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportCaseManagement.Application.Features.SupportTicketList;
using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Models;
using SupportCaseManagement.Presentation.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CasesViewModel(ISupportTicketListService ticketListService, INavigationService navigationService) : ObservableObject
{
    private const string _ErrorMessage = "Ärenden kunde inte läsas in från fil";
    private readonly ISupportTicketListService _ticketListService = ticketListService;
    private readonly INavigationService _navigationService = navigationService;

    public ObservableCollection<SupportTicket> SupportTickets { get; } = [];

    [ObservableProperty]
    public partial string StatusMessage { get; private set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public partial bool IsBusy { get; private set; }

    public bool IsNotBusy => !IsBusy;

    public async Task LoadAllSupportTicketsAsync()
    {
        IsBusy = true;

        try
        {
            IEnumerable<SupportTicket> supportTickets = await _ticketListService.GetAllTicketsSortedAsync();

            SupportTickets.Clear();

            foreach (SupportTicket supportTicket in supportTickets)
            {
                SupportTickets.Add(supportTicket);
            }

            if (SupportTickets.Count > 0)
            {
                StatusMessage = $"{SupportTickets.Count} ärenden i listan";
            }
            else
            {
                StatusMessage = "Det finns inga sparade ärenden än";
            }
        }
        catch
        {
            StatusMessage = _ErrorMessage;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task LoadSupportTicketsFilteredBy(TicketStatus status)
    {
        IsBusy = true;

        try
        {
            IEnumerable<SupportTicket> supportTickets = await _ticketListService.GetTicketsWithAsync(status);

            SupportTickets.Clear();

            foreach (SupportTicket supportTicket in supportTickets)
            {
                SupportTickets.Add(supportTicket);
            }

            if (SupportTickets.Count > 0)
            {
                StatusMessage = $"{SupportTickets.Count} ärenden med status {status}";
            }
            else
            {
                StatusMessage = $"Det finns inga ärenden med status {status}";
            }
        }
        catch
        {
            StatusMessage = _ErrorMessage;
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task LoadSupportTicketsContaining(string text)
    {
        IsBusy = true;

        try
        {
            IEnumerable<SupportTicket> supportTickets = await _ticketListService.GetTicketsContainingAsync(text);

            SupportTickets.Clear();

            foreach (SupportTicket supportTicket in supportTickets)
            {
                SupportTickets.Add(supportTicket);
            }

            if (SupportTickets.Count > 0)
            {
                StatusMessage = $"{SupportTickets.Count} träffar för '{text}'";
            }
            else
            {
                StatusMessage = $"Det finns inga träffar för '{text}'";
            }
        }
        catch
        {
            StatusMessage = _ErrorMessage;
        }
        finally
        {
            IsBusy = false;
        }
    }


    [RelayCommand]
    private void OpenCreateSupportTicket()
    {
        //_navigationService.Navigate(AppPage.);
    }


    [RelayCommand]
    private void OpenSupportTicket(SupportTicket ticket)
    {
        if (ticket is null)
            return;
        _navigationService.Navigate(AppPage.CaseManager, ticket);
    }
}

