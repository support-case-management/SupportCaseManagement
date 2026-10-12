using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.ViewModels;

namespace SupportCaseManagement.Presentation.Pages;

public sealed partial class OverviewView : Page
{
    public OverviewViewModel ViewModel { get; }
    public OverviewView(OverviewViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }

    private async void Page_Loaded(object sender, RoutedEventArgs e)
    {
        await ViewModel.LoadOverviewDataAsync();
    }
}

