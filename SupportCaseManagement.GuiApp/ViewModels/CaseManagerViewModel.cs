using CommunityToolkit.Mvvm.ComponentModel;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CaseManagerViewModel : ObservableObject
{
    [ObservableProperty]
    public partial SupportTicket? SelectedTicket { get; set; }
}

