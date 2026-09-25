using SupportCaseManagement.Presentation.Navigation;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class OverviewViewModel
{
    private readonly INavigationService _navigationService;

    public string Title { get; set; } = "Welcome to the Overview Page!";



    public OverviewViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

    }


}

