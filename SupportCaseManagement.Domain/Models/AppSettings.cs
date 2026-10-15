namespace SupportCaseManagement.Domain.Models;

public class AppSettings
{
    public string CustomerFilePath { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SupportCaseManagement",
        "customers.json");

    public string SupportTicketFilePath { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SupportCaseManagement",
        "supportTickets.json");
}
