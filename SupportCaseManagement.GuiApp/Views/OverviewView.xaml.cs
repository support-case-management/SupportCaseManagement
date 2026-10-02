using Microsoft.Extensions.DependencyInjection;
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
}

