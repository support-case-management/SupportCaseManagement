using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Presentation.ViewModels;

namespace SupportCaseManagement.Presentation.Views;

public sealed partial class CaseManagerView : Page
{
    public CaseManagerViewModel ViewModel { get; }
    public CaseManagerView(CaseManagerViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}
