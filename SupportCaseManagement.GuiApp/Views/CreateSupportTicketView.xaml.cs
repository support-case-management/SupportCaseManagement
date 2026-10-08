using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.ViewModels;

namespace SupportCaseManagement.Presentation.Views;

public sealed partial class CreateSupportTicketView : Page
{
    public CreateSupportTicketViewModel ViewModel { get; }
    public CreateSupportTicketView(CreateSupportTicketViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = ViewModel;

        InitializeComponent();

    }
}

