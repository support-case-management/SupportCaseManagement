using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Domain.Models;
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

    private async void Page_Loaded(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        await ViewModel.LoadAllCustomersAsync();
    }

    private void CustomerList_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is Customer customer)
        {
            ViewModel.OpenUpdateCustomerCommand.Execute(customer);
        }
    }
}

