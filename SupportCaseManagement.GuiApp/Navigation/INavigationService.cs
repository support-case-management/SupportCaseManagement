

using Microsoft.UI.Xaml.Controls;

namespace SupportCaseManagement.Presentation.Navigation;

public interface INavigationService
{
    bool CanGoBack { get; }

    void GoBack();
    void Initalize(Frame frame);
    void Initalize();
    void Navigate(AppPage page, string? parameter = null);
}

