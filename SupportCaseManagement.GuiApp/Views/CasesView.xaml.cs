using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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

    

    private async void SearchTxtb_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        await ViewModel.LoadSupportTicketsContaining(SearchTxtb.Text);
    }

    private void SupportTicketList_ItemClick(object sender, ItemClickEventArgs e)
    {

    }
}
