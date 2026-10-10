using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class CaseManagerViewModel : ObservableObject
{
    private readonly ISupportTicketRepository _ticketRepository;

    public CaseManagerViewModel(ISupportTicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }


    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string Description { get; set; } = string.Empty;
    [ObservableProperty]
    public partial int PriorityIndex { get; set; } = -1;
    [ObservableProperty]
    public partial int StatusIndex { get; set; } = -1;
    [ObservableProperty]
    public partial string? AssignedCaseWorker { get; set; }

    [ObservableProperty]
    public partial string NewComment { get; set; } = string.Empty;
    public IReadOnlyList<Comment> Comments =>
        SelectedTicket?.Comments ?? [];

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CreatedAtText))]
    [NotifyPropertyChangedFor(nameof(Comments))]
    public partial SupportTicket? SelectedTicket { get; set; }

    partial void OnSelectedTicketChanged(SupportTicket? value)
    {
        Title = value?.Title ?? string.Empty;
        Description = value?.Description ?? string.Empty;
        PriorityIndex = value is null ? -1 : (int)value.Priority;
        StatusIndex = value is null ? -1 : (int)value.Status;
        AssignedCaseWorker = value?.AssignedCaseWorker;
    }

    public string CreatedAtText =>
        SelectedTicket is null
        ? string.Empty : SelectedTicket.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm");

    [ObservableProperty]
    public partial string SaveMessage { get; set; } = string.Empty;
    [RelayCommand]
    private async Task SaveAsync()
    {
        if (SelectedTicket is null)
        {
            SaveMessage = "Välj ett ärende först";
            return;
        }
        try
        {
            var updatedTicket = new SupportTicket(
                SelectedTicket.Id,
                Title,
                Description,
                SelectedTicket.CustomerId,
                (TicketPriority)PriorityIndex,
                (TicketStatus)StatusIndex,
                SelectedTicket.CreatedAt,
                AssignedCaseWorker,
                SelectedTicket.Comments
            )
            {
                Customer = SelectedTicket.Customer
            };
            var savedTicket = await _ticketRepository.UpdateAsync(updatedTicket);
            SelectedTicket = savedTicket;
            SaveMessage = "Ändringar sparades";
        }
        catch (Exception ex)
        {
            SaveMessage = $"Fel, kunde inte spara ändringar: {ex.Message}";
        }

    }

    [RelayCommand]
    private async Task AddCommentAsync()
    {
        if (SelectedTicket is null)
        {
            SaveMessage = "Välj ett ärende först";
            return;
        }
        try
        {
            var updatedTicket = new SupportTicket(
                SelectedTicket.Id,
                Title,
                Description,
                SelectedTicket.CustomerId,
                (TicketPriority)PriorityIndex,
                (TicketStatus)StatusIndex,
                SelectedTicket.CreatedAt,
                AssignedCaseWorker,
                SelectedTicket.Comments
            )
            {
                Customer = SelectedTicket.Customer
            };

            updatedTicket.AddComment(NewComment);

            var savedTicket = await _ticketRepository.UpdateAsync(updatedTicket);
            SelectedTicket = savedTicket;
            NewComment = string.Empty;
            SaveMessage = "Kommentar sparades";
        }
        catch (Exception ex)
        {
            SaveMessage = $"Kunde inte spara kommentar: {ex.Message}";
        }

    }




}



