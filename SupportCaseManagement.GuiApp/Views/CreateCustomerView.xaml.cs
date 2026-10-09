using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.ViewModels;

namespace SupportCaseManagement.Presentation.Views;

public sealed partial class CreateCustomerView : Page
{
    public CreateCustomerViewModel ViewModel { get; }

    public CreateCustomerView(CreateCustomerViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}
