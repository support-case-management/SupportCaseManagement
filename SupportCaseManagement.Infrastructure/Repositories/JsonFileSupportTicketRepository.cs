using SupportCaseManagement.Domain.Interfaces;
using System.Text.Json;

namespace SupportCaseManagement.Infrastructure.Repositories;

internal class JsonFileSupportTicketRepository : ISupportTicketRepository
{
    private readonly string _supportTicketsFilePath = Path.Combine
    (
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SupportCaseManagement",
        "supportTickets.json"
    );

    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<SupportTicket>> GetAllAsync()
    {
        if (!File.Exists(_supportTicketsFilePath))
        {
            return [];
        }

        string json = await File.ReadAllTextAsync(_supportTicketsFilePath);

        List<SupportTicket> supportTickets = JsonSerializer.Deserialize < ListzSupportTicket >> (json, _options)
            ?? throw new JsonException("The support ticket file must contain a json based list");

        return supportTickets;
    }

    public async Task SaveAllAsync(IEnumerable<SupportTicket> supportTickets)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_supportTicketsFilePath)!);

        string json = JsonSerializer.Serialize(supportTickets, _options);
        var tempPath = _supportTicketsFilePath + ".tmp";

        await File.WriteAllTextAsync(tempPath, json);
        File.Move(tempPath, _supportTicketsFilePath, overwrite: true);
    }
}
