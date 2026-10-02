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
}
