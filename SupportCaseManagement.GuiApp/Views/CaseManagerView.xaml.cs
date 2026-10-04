
using Microsoft.UI.Xaml.Controls;
using Microsoft.Extensions.DependencyInjection;
using SupportCaseManagement.Application.CaseWorker;
using SupportCaseManagement.Presentation.ViewModels;

namespace SupportCaseManagement.Presentation.Views;

namespace SupportCaseManagement.Presentation.Views
{
 
    public sealed partial class CaseManagerView : Page
    {
        public CaseManagerViewModel ViewModel { get; }
        public CaseManagerView(CaseManagerViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();

            var service = App.Provider.GetRequiredService<CaseWorkerService>();
            CaseWorkerBox.ItemsSource = service.GetAll();
        }
    }
}
