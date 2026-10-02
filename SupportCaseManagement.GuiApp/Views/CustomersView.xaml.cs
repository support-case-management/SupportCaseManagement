using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.ViewModels;

namespace SupportCaseManagement.Presentation.Views;

public sealed partial class CustomersView : Page
{
    public CustomersViewModel ViewModel { get; }

    public CustomersView(CustomersViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}

