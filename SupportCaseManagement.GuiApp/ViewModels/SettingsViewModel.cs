using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SupportCaseManagement.Application.Features.Settings;
using System;
using System.Threading.Tasks;

namespace SupportCaseManagement.Presentation.ViewModels;

public partial class SettingsViewModel(ISettingsService settingsService) : ObservableObject
{
    private readonly ISettingsService _settingsService = settingsService;

    [ObservableProperty]
    public partial string CustomerFilePath { get; set; } = settingsService.CustomerFilePath;

    [ObservableProperty]
    public partial string SupportTicketFilePath { get; set; } = settingsService.SupportTicketFilePath;

    [ObservableProperty]
    public partial string StatusMessage { get; private set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    public partial bool IsBusy { get; private set; }

    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    private async Task SaveCustomerPathAsync()
    {
        IsBusy = true;
        StatusMessage = string.Empty;

        try
        {
            await _settingsService.SetCustomerFilePathAsync(CustomerFilePath);

            CustomerFilePath = _settingsService.CustomerFilePath;
            StatusMessage = "Kundfilens sökväg har sparats.";
        }
        catch (ArgumentException ex)
        {
            StatusMessage = ex.Message;
        }
        catch (Exception)
        {
            StatusMessage = "Kundfilens sökväg kunde inte sparas.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveSupportTicketPathAsync()
    {
        IsBusy = true;
        StatusMessage = string.Empty;

        try
        {
            await _settingsService.SetSupportTicketFilePathAsync(SupportTicketFilePath);

            SupportTicketFilePath = _settingsService.SupportTicketFilePath;
            StatusMessage = "Ärendefilens sökväg har sparats.";
        }
        catch (ArgumentException ex)
        {
            StatusMessage = ex.Message;
        }
        catch (Exception)
        {
            StatusMessage = "Ärendefilens sökväg kunde inte sparas.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
