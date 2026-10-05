using CommunityToolkit.Mvvm.ComponentModel;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CaseManagerViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PriorityIndex))]
    [NotifyPropertyChangedFor(nameof(StatusIndex))]
    [NotifyPropertyChangedFor(nameof(CreatedAtText))]
    public partial SupportTicket? SelectedTicket { get; set; }

    public int PriorityIndex => 
        SelectedTicket is null ? -1 : (int)SelectedTicket.Priority;
    public int StatusIndex => 
        SelectedTicket is null ? -1 : (int)SelectedTicket.Status;

    public string CreatedAtText =>
        SelectedTicket is null 
        ? string.Empty:SelectedTicket.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
}

