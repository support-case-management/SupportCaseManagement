using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Application.CaseWorker;
using SupportCaseManagement.Presentation.ViewModels;

namespace SupportCaseManagement.Presentation.Views;


public sealed partial class CaseManagerView : Page
{
    public CaseDetailViewModel ViewModel { get; }
    public CaseManagerView(CaseDetailViewModel viewModel, CaseWorkerService caseWorkerService)
    {
        ViewModel = viewModel;
        InitializeComponent();

        CaseWorkerBox.ItemsSource = caseWorkerService.GetAll();
    }
}
