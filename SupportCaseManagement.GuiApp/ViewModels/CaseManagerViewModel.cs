using CommunityToolkit.Mvvm.ComponentModel;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CaseManagerViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriorityIndex))]
    [NotifyPropertyChangedFor(nameof(StatusIndex))]
    public partial SupportTicket? SelectedTicket { get; set; }

    public int PriorityIndex => 
        SelectedTicket is null ? -1 : (int)SelectedTicket.Priority;
    public int StatusIndex => 
        SelectedTicket is null ? -1 : (int)SelectedTicket.Status;

}

