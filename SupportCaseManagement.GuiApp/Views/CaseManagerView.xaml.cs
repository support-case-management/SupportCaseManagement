
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using SupportCaseManagement.Application.CaseWorker;
using SupportCaseManagement.Presentation.ViewModels;

namespace SupportCaseManagement.Presentation.Views;


public sealed partial class CaseManagerView : Page
{
    public CaseManagerViewModel ViewModel { get; }
    public CaseManagerView(CaseManagerViewModel viewModel, CaseWorkerService caseWorkerService)
    {
        ViewModel = viewModel;
        InitializeComponent();

        CaseWorkerBox.ItemsSource = caseWorkerService.GetAll();
    }
}
