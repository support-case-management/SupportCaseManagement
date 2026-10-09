using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Models;
using SupportCaseManagement.Presentation.ViewModels;
using System;

namespace SupportCaseManagement.Presentation.Views;

public sealed partial class CasesView : Page
{
    private readonly Func<CreateSupportTicketView> _createTicketViewFactory;
    public CasesViewModel ViewModel { get; }

    public CasesView(CasesViewModel viewModel, Func<CreateSupportTicketView> createTicketViewFactory)
    {
        ViewModel = viewModel;

        InitializeComponent();

        // sätter automatiskt filtret till att visa alla ärenden när vyn öppnas
        FilterCmb.SelectedIndex = 3;

        _createTicketViewFactory = createTicketViewFactory;
    }

    private async void OpenCreateTicketDialog_Click(object sender, RoutedEventArgs e)
    {
        var createTicketView = _createTicketViewFactory();

        var dialog = new ContentDialog
        {
            Title = "Registrera nytt ärende",
            Content = createTicketView,
            CloseButtonText = "Avbryt",
            XamlRoot = this.XamlRoot
        };

        await dialog.ShowAsync();

        await ViewModel.LoadAllSupportTicketsAsync();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadAllSupportTicketsAsync();
    }

    private async void SearchTxtb_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = ((TextBox)sender).Text;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            await ViewModel.LoadSupportTicketsContaining(searchText);
        }
        else if (ViewModel.IsFilterOn)
        {
            await ViewModel.LoadSupportTicketsFilteredBy((TicketStatus)FilterCmb.SelectedIndex);
        }
        else
        {
            await ViewModel.LoadAllSupportTicketsAsync();
        }

        SearchTxtb.Focus(FocusState.Programmatic);
    }

    private void SupportTicketList_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is SupportTicket ticket)
        {
            ViewModel.OpenSupportTicketCommand.Execute(ticket);
        }
    }

    private async void FilterCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FilterCmb.SelectedIndex == 3)
        {
            await ViewModel.LoadAllSupportTicketsAsync();
        }
        else
        {
            await ViewModel.LoadSupportTicketsFilteredBy((TicketStatus)FilterCmb.SelectedIndex);
        }
    }
}
