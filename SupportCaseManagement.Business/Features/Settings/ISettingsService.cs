namespace SupportCaseManagement.Application.Features.Settings
{
    public interface ISettingsService
    {
        string CustomerFilePath { get; }
        string SupportTicketFilePath { get; }

        Task SetCustomerFilePathAsync(string filePath);
        Task SetSupportTicketFilePathAsync(string filePath);
    }
}
