using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;
using System.Text.Json;

namespace SupportCaseManagement.Infrastructure.Repositories;

/// <summary>
/// Implementation av ISupportTicketRepository, som sparar och hämtar en lista med ärenden till och från en json-fil
/// </summary>
internal class JsonFileSupportTicketRepository : ISupportTicketRepository
{
    // skapar en filepath genom att kombinera flera delar
    private readonly string _supportTicketsFilePath = Path.Combine
    (
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "SupportCaseManagement",
        "supportTickets.json"
    );

    // sätter "inställningar" för hur json filen ska sparas, sätter:
    // WriteIndented till true för att allt inte ska stå på en och samma rad,
    // PropertyNameCaseInsensitive till true för att den inte ska bry sig om det är stora eller små bokstäver
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Sparar den givna listan av ärenden i en json-fil med den förbestämda filpath:en.
    /// Den är async och använder sig av await för att applikationen inte ska låsas helt från all interaktion medans metoden körs.
    /// </summary>
    /// <param name="supportTickets">den lista av ärenden som ska sparas till json-fil</param>
    public async Task SaveAllAsync(IEnumerable<SupportTicket> supportTickets)
    {
        // CreateDirectory - skapar alla mappar och filen som står i filepath:en, om de inte redan existerar
        Directory.CreateDirectory(Path.GetDirectoryName(_supportTicketsFilePath)!);

        string json = JsonSerializer.Serialize(supportTickets, _options);
        var tempPath = _supportTicketsFilePath + ".tmp";

        // lägger ärendena i temp-filen först ifall något går fel, flyttar sen över till den riktiga filen när vi vet att det fungerade
        await File.WriteAllTextAsync(tempPath, json);
        File.Move(tempPath, _supportTicketsFilePath, overwrite: true);
    }

    /// <summary>
    /// Hämtar alla sparade ärenden från den förbestämda filepath:en och returnerar en lista med dessa om den lyckas.
    /// Den är async och använder await för att applikationen inte ska låsas helt från all interaktion medans metoden körs.
    /// </summary>
    /// <returns>en lista med alla sparade ärenden</returns>
    public async Task<List<SupportTicket>> GetAllAsync()
    {
       if (!File.Exists(_supportTicketsFilePath))
        {
            return [];
        }

        string json = await File.ReadAllTextAsync(_supportTicketsFilePath);

        List<SupportTicket> supportTickets = JsonSerializer.Deserialize <List<SupportTicket>>(json, _options)
            ?? throw new JsonException("Ärendelistans fil måste innehålla en json baserad lista");

        return supportTickets;
    }


    /// <summary>
    /// Hämtar ett ärende med det givna id:t från den sparade listan av ärenden
    /// </summary>
    /// <param name="id">id:t på det ärende som ska hämtas ur den sparade listan</param>
    /// <returns>ärendet med det givna id:t från listan</returns>
    public async Task<SupportTicket> GetByIdAsync(Guid id)
    {
        List<SupportTicket> supportTickets = await GetAllAsync();

        SupportTicket? supportTicket = supportTickets.FirstOrDefault(ticket => ticket.Id == id);

        if (supportTicket == null)
        {
            throw new ArgumentNullException($"Inget ärende med id '{id}' kunde hittas i den sparade filen");
        }

        return supportTicket;
    }

    /// <summary>
    /// Uppdaterar ett ärende i den sparade listan av ärenden till det givna ärendet
    /// </summary>
    /// <param name="updatedTicket">det uppdaterade ärendet</param>
    /// <returns>det uppdaterade ärendet</returns>
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
