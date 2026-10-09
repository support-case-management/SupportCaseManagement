using CommunityToolkit.Mvvm.ComponentModel;
using SupportCaseManagement.Application.Features.Customers;
using SupportCaseManagement.Presentation.Navigation;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CreateCustomerViewModel(ICustomerService customerService, INavigationService navigationService) : ObservableObject
{
    private readonly ICustomerService _customerService = customerService;
    private readonly INavigationService _navigationService = navigationService;


    [ObservableProperty]
    public partial string StatusMessage { get; private set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public partial bool IsBusy { get; private set; }

    public bool IsNotBusy => !IsBusy;


}
