using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.ViewModels;
using System.Threading.Tasks;


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
}
