using Microsoft.UI.Xaml.Controls;
using SupportCaseManagement.Domain.Models;
using System.ComponentModel;

namespace SupportCaseManagement.Presentation.Navigation;

public interface INavigationService : INotifyPropertyChanged
{
    Page? CurrentPage { get; }
    void Navigate(AppPage page, SupportTicket? ticket = null);
}

