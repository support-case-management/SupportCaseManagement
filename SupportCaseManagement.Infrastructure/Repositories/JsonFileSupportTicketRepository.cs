using SupportCaseManagement.Application.Features.Settings;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;
using System.Text.Json;

namespace SupportCaseManagement.Infrastructure.Repositories;

internal class JsonFileSupportTicketRepository(ISettingsService settingsService) : ISupportTicketRepository
{
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };


    public async Task SaveAllAsync(IEnumerable<SupportTicket> supportTickets)
    {
        var filePath = settingsService.SupportTicketFilePath;


        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        string json = JsonSerializer.Serialize(supportTickets, _options);
        var tempPath = filePath + ".tmp";


        await File.WriteAllTextAsync(tempPath, json);
        File.Move(tempPath, filePath, overwrite: true);
    }


    public async Task<List<SupportTicket>> GetAllAsync()
    {

        var filePath = settingsService.SupportTicketFilePath;

        if (!File.Exists(filePath))
        {
            return [];
        }

        string json = await File.ReadAllTextAsync(filePath);

        List<SupportTicket> supportTickets = JsonSerializer.Deserialize<List<SupportTicket>>(json, _options)
            ?? throw new JsonException("Ärendelistans fil måste innehålla en json baserad lista");

        return supportTickets;
    }



    public async Task<SupportTicket> GetByIdAsync(Guid id)
    {
        List<SupportTicket> supportTickets = await GetAllAsync();

        SupportTicket? supportTicket = supportTickets.FirstOrDefault(ticket => ticket.Id == id);

        if (supportTicket is null)
        {
            throw new ArgumentNullException($"Inget ärende med id '{id}' kunde hittas i den sparade filen");
        }

        return supportTicket;
    }
    public async Task<SupportTicket> UpdateAsync(SupportTicket updatedTicket)
    {
        if (updatedTicket is null)
        {
            throw new ArgumentNullException("Ärendet kan inte ändras till null");
        }

        List<SupportTicket> supportTickets = await GetAllAsync();
        int index;

        for (index = 0; index < supportTickets.Count; index++)
        {
            if (supportTickets[index].Id == updatedTicket.Id)
            {
                supportTickets[index] = updatedTicket;
                break;
            }
        }

        if (index == supportTickets.Count)
        {
            throw new InvalidOperationException("Ärendet kunde inte hittas i den sparade filen");
        }

        await SaveAllAsync(supportTickets);

        return supportTickets[index];
    }
}
