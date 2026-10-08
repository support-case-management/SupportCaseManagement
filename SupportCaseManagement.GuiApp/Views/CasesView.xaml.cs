using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Domain.Models;
using SupportCaseManagement.Presentation.ViewModels;

namespace SupportCaseManagement.Presentation.Views;

public sealed partial class CasesView : Page
{
    public CasesViewModel ViewModel { get; }

    public CasesView(CasesViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAllSupportTicketsAsync();
    }


   private void SupportTickets_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is SupportTicket ticket)
        {
            ViewModel.OpenSupportTicketCommand.Execute(ticket);
        }
    }

}
