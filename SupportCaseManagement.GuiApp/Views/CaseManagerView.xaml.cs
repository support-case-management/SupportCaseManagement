using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Application.CaseWorker;

namespace SupportCaseManagement.Presentation.Views;

public sealed partial class CaseManagerView : Page
{
    public CaseManagerView()
    {
        InitializeComponent();

        var service = App.Provider.GetRequiredService<CaseWorkerService>();
        CaseWorkerBox.ItemsSource = service.GetAll();

    }
}
